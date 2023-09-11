namespace TestWebApp.Application.Transactions.Commands
{
    using FluentValidation;
    using MediatR;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Domain;

    public class CreateTransactionCommand : IRequest
    {
        public Guid From { get; set; }

        public Guid To { get; set; }

        public decimal Amount { get; set; }
    }

    internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public IUnitOfWork unitOfWork;

        public CreateTransactionCommandValidator(IUnitOfWork accounts)
        {
            this.unitOfWork = accounts;
            RuleFor(t => t.From).NotEmpty()
                                .Must((t, _) => DifferentAccounts(t)).WithMessage("Receiving and sending accounts must be different.")
                                .MustAsync(AccountExists);
            RuleFor(t => t.To).NotEmpty().MustAsync(AccountExists);
            RuleFor(t => t.Amount).GreaterThan(0m);

        }

        public bool DifferentAccounts(CreateTransactionCommand t)
        {
            return t.From != t.To;
        }

        public async Task<bool> AccountExists(Guid id, CancellationToken cancellationToken)
        {
            Account? account = await unitOfWork.Accounts.GetByIdAsync(id, cancellationToken);
            return account is not null;
        }
    }

    internal sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            Transaction t = new Transaction();

            Task<Account> from = unitOfWork.Accounts.GetByIdSafeAsync(request.From, cancellationToken);
            Task<Account> to = unitOfWork.Accounts.GetByIdSafeAsync(request.To, cancellationToken);

            t.Id = Guid.NewGuid();
            t.From = await from;
            t.FromId = t.From.Id;
            t.To = await to;
            t.ToId = t.To.Id;
            t.Amount = request.Amount;
            t.CreatedAt = DateTime.UtcNow;

            unitOfWork.Transactions.Create(t);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
