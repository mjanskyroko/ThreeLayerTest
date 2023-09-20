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

    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly DbSet<Account> accounts;

        public AccountRepository(MssqlDbContext context)
        {
            this.accounts = context.Set<Account>();
        }

        public void Create(Account a)
        {
            accounts.Add(a);
        }

        public void Update(Account a)
        {
            accounts.Update(a);
        }

        public void Delete(Account a)
        {
            accounts.Remove(a);
        }

        public async Task<List<Account>> GetAsync(AccountFilter filter, CancellationToken cancellationToken)
        {
            return await accounts.WhereIf(filter.OwnerId is not null, a => a.OwnerId == filter.OwnerId)
                .WhereIf(filter.BalanceMinimum is not null, a => a.Balance >= filter.BalanceMinimum)
                .WhereIf(filter.BalanceMaximum is not null, a => a.Balance <= filter.BalanceMaximum)
                .WhereIf(filter.Name is not null, a => a.Name == filter.Name)
                .WhereIf(filter.IsActive is not null, a => a.IsActive == filter.IsActive)
                .Include(a => a.Owner)
                .Skip(filter.Offset).Take(filter.Limit).ToListAsync(cancellationToken);
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await accounts.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Account> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await accounts.FindAsync(new object[] { id }, cancellationToken) ?? throw new ApplicationException($"Unable to find Account {id}.");
        }
    }
}
