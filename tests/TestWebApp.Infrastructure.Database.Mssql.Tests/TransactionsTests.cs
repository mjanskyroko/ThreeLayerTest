namespace TestWebApp.Infrastructure.Database.Mssql.Tests
{
    using System;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Threading;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Fixtures;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql;
    using Xunit;

    public class TransactionsTests : IClassFixture<MssqlDatabaseFixture>
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionsTests(MssqlDatabaseFixture dbFixture)
        {
            transactionRepository = new TransactionRepository(dbFixture.DbContext);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void TransactionCount()
        {
            TransactionFilter filter = new TransactionFilter();
            filter.Limit = 100;
            var transactions = await transactionRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(3, transactions.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void AmountFilter()
        {
            TransactionFilter filter = new TransactionFilter();
            filter.Limit = 100;
            filter.AmountMinimum = 600m;
            filter.AmountMaximum = 1800m;
            var transactions = await transactionRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void DateFilter()
        {
            TransactionFilter filter = new TransactionFilter();
            filter.Limit = 100;
            filter.DateFrom = new System.DateTime(2023, 9, 15, 0, 0, 0, System.DateTimeKind.Utc);
            filter.DateTo = new System.DateTime(2023, 9, 16, 0, 0, 0, System.DateTimeKind.Utc);
            var transactions = await transactionRepository.GetAsync(filter, new CancellationToken());
            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void AccountFromFilter()
        {
            TransactionFilter filter = new TransactionFilter();
            filter.Limit = 100;
            filter.AccountFrom = Guid.Parse("1b8d6b4d-64c4-4161-b17f-32a43ece888c");
            var transactions = await transactionRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(transactions);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void AccountToFilter()
        {
            TransactionFilter filter = new TransactionFilter();
            filter.Limit = 100;
            filter.AccountTo = Guid.Parse("9a0687e6-96ee-4429-87fc-aadecc138add");
            var transactions = await transactionRepository.GetAsync(filter, new CancellationToken());
            Assert.Single(transactions);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void WithAccount()
        {
            Guid accId = Guid.Parse("c77f4638-cdde-49c7-a24a-f6d858ff6394");
            var transactions = await transactionRepository.GetWithAccountAsync(accId, new CancellationToken());
            Assert.Equal(2, transactions.Count);
            Assert.True(transactions.First().Id != transactions.Last().Id);
        }

        [Fact]
        [MssqlSeed("Seeds/transactions-db.json")]
        public async void WithUser()
        {
            Guid userId = Guid.Parse("5c1bf8ac-9be4-468c-a639-2f34e26cf232");
            var transactions = await transactionRepository.GetWithUserAsync(userId, new CancellationToken());
            var distinct = transactions.DistinctBy(x => x.CreatedAt).ToList();
            Assert.Equal(distinct.Count, transactions.Count);
        }
    }
}
