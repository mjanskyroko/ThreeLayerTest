namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Extensions;

    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly DbSet<Transaction> transactions;

        public TransactionRepository(MssqlDbContext context)
        {
            transactions = context.Set<Transaction>();
        }

        public void Create(Transaction u)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Transaction>> GetAsync(TransactionFilter filter, CancellationToken cancellationToken)
        {
            return await transactions.WhereIf(filter.AccountFrom is not null, t => t.From.Id == filter.AccountFrom)
                .WhereIf(filter.AccountTo is not null, t => t.To.Id == filter.AccountTo)
                .WhereIf(filter.MinAmount is not null, t => t.Amount >= filter.MinAmount)
                .WhereIf(filter.MaxAmount is not null, t => t.Amount <= filter.MaxAmount)
                .Skip(filter.Offset).Take(filter.Limit).ToListAsync(cancellationToken);
        }

        public async Task<Transaction> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await transactions.FindAsync(new object[] { id }, cancellationToken) ?? throw new ApplicationException($"Unable to find Transaction {id}");
        }

        public async Task<List<Transaction>> GetWithAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await transactions.Where(t => t.From.Id == accountId || t.To.Id == accountId).ToListAsync(cancellationToken);
        }
    }
}
