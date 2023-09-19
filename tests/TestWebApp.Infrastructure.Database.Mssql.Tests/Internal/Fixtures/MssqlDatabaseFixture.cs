namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Fixtures
{
    using DotNet.Testcontainers.Builders;
    using Microsoft.EntityFrameworkCore;
    using System;
    using Testcontainers.MsSql;
    using TestWebApp.Infrastructure.Database.Mssql.Internal;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql;

    public class MssqlDatabaseFixture : IDisposable
    {
        private bool disposed;
        private readonly IEnvironmentVariableManager eventVariableManager = new MssqlEnvironmentVariableManager();
        internal MssqlDbContext DbContext { get; }
        public MsSqlContainer? MssqlDatabase { get; }

        public MssqlDatabaseFixture()
        {
            disposed = false;

            string? connStr = null;
            if (MssqlContainerConstants.UseContainer)
            {
                MssqlDatabase = new MsSqlBuilder()
                    .WithImage(MssqlContainerConstants.Image)
                    .WithPassword(MssqlContainerConstants.Password)
                    .WithExposedPort(MssqlContainerConstants.Port)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MssqlContainerConstants.Port))
                    .Build();

                MssqlDatabase.StartAsync().Wait();

                connStr = MssqlDatabase.GetConnectionString();
            }
            else
            {
                connStr = $"Server=127.0.0.1,{MssqlContainerConstants.Port};Database={MssqlContainerConstants.Database};User Id={MssqlContainerConstants.Username};Password={MssqlContainerConstants.Password};TrustServerCertificate=Yes";
            }

            var options = new DbContextOptionsBuilder<MssqlDbContext>().UseSqlServer(connStr).Options;

            DbContext = new MssqlDbContext(options);
            DbContext.Database.Migrate();
            eventVariableManager.Set(connStr);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (MssqlDatabase is not null)
                        MssqlDatabase.DisposeAsync().AsTask().Wait();
                    DbContext.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
