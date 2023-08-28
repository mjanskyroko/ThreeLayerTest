namespace TestWebApp.Infrastructure.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;
    using TestWebApp.Application.Contracts.Services;
    using TestWebApp.Infrastructure.Services.Internal;

    public static class ServicesModule
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();
            return services;
        }
    }
}
