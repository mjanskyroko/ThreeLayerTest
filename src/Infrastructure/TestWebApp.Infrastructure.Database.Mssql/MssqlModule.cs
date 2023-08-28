namespace TestWebApp.Infrastructure.Database.Mssql
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Infrastructure.Database.Mssql.Internal;
    using TestWebApp.Infrastructure.Database.Mssql.Internal.Repositories;

    public static class MssqlModule
    {
        public static IServiceCollection AddMssqlDatabaseLayer(this IServiceCollection services, MssqlAdapterSettings settings)
        {
            services.AddDbContext<MssqlDbContext>(options =>
            {
                options.UseSqlServer(settings.Url);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }

        public static IApplicationBuilder MigrateMssqlDb(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetService<MssqlDbContext>();

            if (dbContext is null)
            {
                throw new ApplicationException(nameof(dbContext));
            }

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            return builder;
        }
    }

    public class MssqlAdapterSettings
    {
        public const string Key = nameof(MssqlAdapterSettings);

        public string Url { get; set; } = default!;
    }

}
