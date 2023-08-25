using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestWebApp.Application.Contracts.Database.Models;
using TestWebApp.Domain;

namespace TestWebApp.Application.Contracts.Database
{
    public interface ITransactionRepository
    {
        void Create(Transaction u);

        Task<Transaction> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Transaction>> GetAsync(TransactionFilter filter, CancellationToken cancellationToken);
    }
}
