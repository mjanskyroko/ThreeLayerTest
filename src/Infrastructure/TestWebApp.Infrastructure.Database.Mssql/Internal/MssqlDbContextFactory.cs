﻿namespace TestWebApp.Infrastructure.Database.Mssql.Internal
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    internal sealed class MssqlDbContextFactory : IDesignTimeDbContextFactory<MssqlDbContext>
    {
        public MssqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MssqlDbContext>();

            optionsBuilder.UseSqlServer(null);

            var instance = new MssqlDbContext(optionsBuilder.Options);

            return instance is null ? throw new InvalidOperationException($"Unable to initialize {nameof(MssqlDbContext)} instance.") : instance;

        }
    }
}
