namespace TestWebApp.Infrastructure.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System;
    using System.Reflection;
    using TestWebApp.Application.Contracts.Services;
    using TestWebApp.Infrastructure.Services.Internal;

    internal sealed class ServicesSettingsConfig : IConfigureOptions<ServicesSettings>
    {
        private readonly IConfiguration config;
        public ServicesSettingsConfig(IConfiguration config)
        {
            this.config = config;
        }

        public void Configure(ServicesSettings options)
        {
            config.GetSection(ServicesSettings.Key).Bind(options);
        }
    }

    public static class ServicesModule
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();
            services.ConfigureOptions<ServicesSettingsConfig>();

            return services;
        }
    }

    public class ServicesSettings
    {
        public const string Key = nameof(ServicesSettings);

        public int SaltLength { get; set; } = 12;

        public int HashCount { get; set; } = 128;
    }
}
