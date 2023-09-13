namespace TestWebApp.Application.Transactions.Commands
{
    using FluentValidation;
    using MediatR;
    using System.Threading;
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
                                .MustAsync((t, _, token) => AccountExistsAndHasMeans(t, token));
            RuleFor(t => t.To).NotEmpty().Must((t, _) => DifferentAccounts(t)).WithMessage("Receiving and sending accounts must be different.")
                                .MustAsync(AccountExists);
            RuleFor(t => t.Amount).GreaterThan(0m);

        }

        private bool DifferentAccounts(CreateTransactionCommand t)
        {
            return t.From != t.To;
        }

        private async Task<bool> AccountExistsAndHasMeans(CreateTransactionCommand t, CancellationToken cancellationToken)
        {
            Account? account = await unitOfWork.Accounts.GetByIdAsync(t.From, cancellationToken);
            return account is not null && t.Amount <= account.Balance;
        }

        private async Task<bool> AccountExists(Guid id, CancellationToken cancellationToken)
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

            Task<Account> taskFrom = unitOfWork.Accounts.GetByIdSafeAsync(request.From, cancellationToken);
            Task<Account> taskTo = unitOfWork.Accounts.GetByIdSafeAsync(request.To, cancellationToken);

            Account from = await taskFrom;
            Account to = await taskTo;

            t.Id = Guid.NewGuid();
            t.From = from;
            t.FromId = from.Id;
            t.To = to;
            t.ToId = to.Id;
            t.Amount = request.Amount;
            t.CreatedAt = DateTime.UtcNow;

            from.Balance -= request.Amount;
            to.Balance += request.Amount;

            unitOfWork.Accounts.Update(from);
            unitOfWork.Accounts.Update(to);
            unitOfWork.Transactions.Create(t);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
