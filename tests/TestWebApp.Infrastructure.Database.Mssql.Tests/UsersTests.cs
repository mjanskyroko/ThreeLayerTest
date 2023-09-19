namespace TestWebApp.Infrastructure.Database.Mssql.Tests
{
    using System.Threading;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Fixtures;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql;
    using Xunit;

    [Collection(CollectionFixtureConstants.Integration)]
    public class UsersTests : IClassFixture<MssqlDatabaseFixture>
    {
        private readonly IUserRepository userRepository;

        public UsersTests(MssqlDatabaseFixture dbFixture)
        {
            userRepository = new UserRepository(dbFixture.DbContext);
        }

        [Fact]
        [MssqlSeed("Seeds/users-db.json")]
        public async void UserCount()
        {
            UserFilter filter = new UserFilter();
            filter.Limit = 100;
            var users = await userRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(2, users.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/users-db.json")]
        public async void NameFilter()
        {
            UserFilter filter = new UserFilter();
            filter.Name = "testuser";
            filter.Limit = 100;
            var users = await userRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(users);
        }

        [Fact]
        [MssqlSeed("Seeds/users-db.json")]
        public async void DateFilter()
        {
            UserFilter filter = new UserFilter();
            filter.JoinDateFrom = new System.DateTime(2023, 9, 11, 12, 0, 0, System.DateTimeKind.Utc);
            filter.JoinDateTo = new System.DateTime(2023, 9, 11, 13, 0, 0, System.DateTimeKind.Utc);
            filter.Limit = 100;
            var users = await userRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(users);
        }
    }
}
