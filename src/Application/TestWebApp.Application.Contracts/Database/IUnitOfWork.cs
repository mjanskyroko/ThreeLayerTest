namespace TestWebApp.Application.Contracts.Database
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);

        IUserRepository Users { get; }

        IAccountRepository Accounts { get; }

        ITransactionRepository Transactions { get; }
    }
}
