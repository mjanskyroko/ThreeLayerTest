namespace TestWebApp.Infrastructure.Database.Mssql.Internal
{
    using TestWebApp.Application.Contracts.Database;

    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly MssqlDbContext context;

        public UnitOfWork(IUserRepository users, IAccountRepository accounts, ITransactionRepository transactions, MssqlDbContext context)
        {
            this.context = context;
            Users = users;
            Accounts = accounts;
            Transactions = transactions;
        }

        public IUserRepository Users { get; }

        public IAccountRepository Accounts { get; }

        public ITransactionRepository Transactions { get; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
