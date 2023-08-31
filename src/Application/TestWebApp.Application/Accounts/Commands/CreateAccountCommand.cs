namespace TestWebApp.Application.Accounts.Commands
{
    using FluentValidation;
    using MediatR;
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

        public CreateAccountCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(a => a.Owner).NotEmpty().MustAsync(OwnerExists).WithMessage("Account owner does not exist.");
            RuleFor(a => a.Name).NotEmpty();
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
            Account a = new Account();
            a.Id = Guid.NewGuid();
            a.Owner = await unitOfWork.Users.GetByIdAsync(request.Owner, cancellationToken);
            a.Name = request.Name;
            a.Balance = 0m;

            unitOfWork.Accounts.Create(a);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
