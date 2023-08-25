namespace TestWebApp.Application.Contracts.Database
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;

    public interface IUserRepository
    {
        void Create(User u);

        void Update(User u);

        void Delete(User u);

        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<User> GetByNameAsync(string name, CancellationToken cancellationToken);

        Task<List<User>> GetAsync(UserFilter filter, CancellationToken cancellationToken);
    }
}
