namespace TestWebApp.Application.Contracts.Database
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;

    public interface ITransactionRepository
    {
        void Create(Transaction u);

        Task<Transaction> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Transaction>> GetAsync(TransactionFilter filter, CancellationToken cancellationToken);

        Task<List<Transaction>> GetWithAccountAsync(Guid accountId, CancellationToken cancellationToken);

        Task<List<Transaction>> GetWithUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}
