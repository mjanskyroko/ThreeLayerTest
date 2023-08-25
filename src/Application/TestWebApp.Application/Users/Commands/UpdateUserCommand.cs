namespace TestWebApp.Application.Users.Commands
{
    using FluentValidation;
    using MediatR;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Internal;
    using TestWebApp.Domain;

    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Password { get; set; } = default!;
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        UpdateUserCommandValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            When(u => u.Password is not null && u.Password.Length > 0, () =>
                {
                    RuleFor(u => u.Password).MinimumLength(8);
                });
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User u = await unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);

            if (request.Password is not null && request.Password.Length > 0)
            {
                u.PasswordSalt = Helpers.RandomBase64String(12);
                u.PasswordHash = Helpers.HashString(request.Password + u.PasswordSalt);
            }

            unitOfWork.Users.Update(u);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
