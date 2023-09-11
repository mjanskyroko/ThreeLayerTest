namespace TestWebApp.Application.Accounts.Commands
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Domain;

    public class CreateAccountCommand : IRequest
    {
        public Guid Owner { get; set; }

        public string Name { get; set; } = default!;
    }

    internal sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ValidationSettings opts;

        public CreateAccountCommandValidator(IUnitOfWork unitOfWork, IOptions<ValidationSettings> options)
        {
            this.unitOfWork = unitOfWork;
            this.opts = options.Value;
            RuleFor(a => a.Owner).NotEmpty().MustAsync(OwnerExists).WithMessage("Account owner does not exist.");
            RuleFor(a => a.Name).NotEmpty().MaximumLength(opts.MaximumAccountNameLength);
        }

        public async Task<bool> OwnerExists(Guid Id, CancellationToken cancellationToken)
        {
            User? owner = await unitOfWork.Users.GetByIdAsync(Id, cancellationToken);
            return owner is not null;
        }
    }

    internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            User u = await unitOfWork.Users.GetByIdSafeAsync(request.Owner, cancellationToken);
            Account a = new Account();
            a.Id = Guid.NewGuid();
            a.Owner = u;
            a.OwnerId = u.Id;
            a.Name = request.Name;
            a.Balance = 0m;
            a.IsActive = true;

            unitOfWork.Accounts.Create(a);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
