namespace TestWebApp.Application.Users.Commands
{
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Internal;
    using TestWebApp.Domain;

    public class CreateUserCommand : IRequest
    {
        public string Name { get; set; } = default!;

        public string Password { get; set; } = default!;
    }

    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(u => u.Name).NotEmpty().MinimumLength(3).MustAsync(IsUniqueName).WithMessage("Username already in use.");
            RuleFor(u => u.Password).MinimumLength(8);
        }

        private async Task<bool> IsUniqueName(string name, CancellationToken cancellationToken)
        {
            return await unitOfWork.Users.GetByNameAsync(name, cancellationToken) == null;
        }
    }

    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User u = new User();

            u.Id = Guid.NewGuid();
            u.Name = request.Name;
            u.PasswordSalt = Helpers.RandomBase64String(12);
            u.PasswordHash = Helpers.HashString(request.Password + u.PasswordSalt);
            u.CreatedAt = DateTime.UtcNow;

            unitOfWork.Users.Create(u);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
