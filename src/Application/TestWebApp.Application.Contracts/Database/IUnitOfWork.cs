using System.Threading;
using System.Threading.Tasks;

namespace TestWebApp.Application.Contracts.Database
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);

        IUserRepository Users { get; }

        IAccountRepository Accounts { get; }

        ITransactionRepository Transactions { get; }
    }
}
