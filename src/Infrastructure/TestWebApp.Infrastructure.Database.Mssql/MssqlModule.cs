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
        public static IServiceCollection AddPostgresDatabaseLayer(this IServiceCollection services, PostgresAdapterSettings settings)
        {
            services.AddDbContext<MssqlDbContext>(options =>
            {
                options.UseSqlServer(settings.Url);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IUserRepository, UserRepository>();

            // TODO: Register repositories.

            return services;
        }

        public static IApplicationBuilder MigratePostgresDb(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetService<MssqlDbContext>();

            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (dbContext.Database.GetPendingMigrations().Count() > 0)
            {
                dbContext.Database.Migrate();
            }

            return builder;
        }
    }

    public class PostgresAdapterSettings
    {
        public const string Key = nameof(PostgresAdapterSettings);

        public string Url { get; set; } = default!;
    }

}
