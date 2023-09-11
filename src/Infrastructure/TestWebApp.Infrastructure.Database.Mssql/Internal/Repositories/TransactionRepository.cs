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
        private readonly DbSet<Account> accounts;

        public TransactionRepository(MssqlDbContext context)
        {
            transactions = context.Set<Transaction>();
            accounts = context.Set<Account>();
        }

        public void Create(Transaction u)
        {
            transactions.Add(u);
        }

        public async Task<List<Transaction>> GetAsync(TransactionFilter filter, CancellationToken cancellationToken)
        {
            return await transactions
                .WhereIf(filter.AccountFrom is not null, t => t.From.Id == filter.AccountFrom)
                .WhereIf(filter.AccountTo is not null, t => t.To.Id == filter.AccountTo)
                .WhereIf(filter.AmountMinimum is not null, t => t.Amount >= filter.AmountMinimum)
                .WhereIf(filter.AmountMaximum is not null, t => t.Amount <= filter.AmountMaximum)
                .WhereIf(filter.DateFrom is not null, t => t.CreatedAt >= filter.DateFrom)
                .WhereIf(filter.DateTo is not null, t => t.CreatedAt <= filter.DateTo)
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

        public async Task<List<Transaction>> GetWithUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            List<Account> accs = await accounts.Where(a => a.Owner.Id == userId).ToListAsync(cancellationToken);

            List<Transaction> transactions = new List<Transaction>(accs.Capacity);
            foreach (var account in accs)
                transactions.AddRange(await GetWithAccountAsync(account.Id, cancellationToken));
            return transactions;
        }
    }
}
