namespace TestWebApp.Infrastructure.Database.Mssql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Fixtures;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql;
    using Xunit;

    public class AccountsTest : IClassFixture<MssqlDatabaseFixture>
    {
        private readonly AccountRepository accountRepository;

        public AccountsTest(MssqlDatabaseFixture dbFixture)
        {
            accountRepository = new AccountRepository(dbFixture.DbContext);
        }

        [Fact]
        [MssqlSeed("Seeds/accounts-db.json")]
        public async void AccountCountTest()
        {
            AccountFilter filter = new AccountFilter();
            filter.Limit = 100;
            var accs = await accountRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(3, accs.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/accounts-db.json")]
        public async void AccoutNameFilterTest()
        {
            AccountFilter filter = new AccountFilter();
            filter.Limit = 100;
            filter.Name = "testacc";
            var accs = await accountRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(accs);
        }

        [Fact]
        [MssqlSeed("Seeds/accounts-db.json")]
        public async void AccountBalanceFilterTest()
        {
            AccountFilter filter = new AccountFilter();
            filter.Limit = 100;
            filter.BalanceMinimum = 13m;
            filter.BalanceMaximum = 1741m;
            var accs = await accountRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(2, accs.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/accounts-db.json")]
        public async void AccountActiveFilterTest()
        {
            AccountFilter filter = new AccountFilter();
            filter.Limit = 100;
            filter.IsActive = false;
            var accs = await accountRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(accs);
        }

        [Fact]
        [MssqlSeed("Seeds/accounts-db.json")]
        public async void AccountOwnerFilterTest()
        {
            AccountFilter filter = new AccountFilter();
            filter.Limit = 100;
            filter.OwnerId = Guid.Parse("5c1bf8ac-9be4-468c-a639-2f34e26cf232");
            var accs = await accountRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(2, accs.Count);
        }
    }
}
