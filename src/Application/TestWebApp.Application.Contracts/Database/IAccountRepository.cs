namespace TestWebApp.Application.Contracts.Database
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Domain;

    public interface IAccountRepository
    {
        void Create(Account a);

        void Update(Account a);

        void Delete(Account a);

        Task<Account> GetByIdAsync(Guid Id, CancellationToken cancellationToken);

        Task<Account> GetAsync();
    }
}
