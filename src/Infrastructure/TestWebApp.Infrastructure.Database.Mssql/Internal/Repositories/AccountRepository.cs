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

    public sealed class AccountRepository : IAccountRepository
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
            return await accounts.WhereIf(filter.OwnerId is not null, a => a.Owner.Id == filter.OwnerId)
                .WhereIf(filter.Name is not null, a => a.Name == filter.Name)
                .WhereIf(filter.Balance is not null, a => a.Balance == filter.Balance)
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
