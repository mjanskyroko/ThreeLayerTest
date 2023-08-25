namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Extensions;

    public sealed class UserRepository : IUserRepository
    {
        private readonly DbSet<User> users;

        public UserRepository(MssqlDbContext context)
        {
            users = context.Set<User>();
        }

        public void Create(User u)
        {
            users.Add(u);
        }

        public void Update(User u)
        {
            users.Update(u);
        }

        public void Delete(User u)
        {
            users.Remove(u);
        }

        public async Task<List<User>> GetAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            return await users.WhereIf(filter.Name is not null, u => u.Name.StartsWith(filter.Name!))
                .WhereIf(filter.JoinDate is not null, u => u.CreatedAt >= filter.JoinDate!)
                .Skip(filter.Offset).Take(filter.Limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await users.FindAsync(new object[] { id }, cancellationToken) ?? throw new ApplicationException($"Unable to find user {id}.");
        }

        public async Task<User> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await users.SingleOrDefaultAsync(u => u.Name == name, cancellationToken);
        }
    }
}
