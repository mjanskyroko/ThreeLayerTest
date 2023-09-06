namespace TestWebApp.Application
{
    using FluentValidation;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Reflection;
    using TestWebApp.Application.Internal.Behaviors;

    internal sealed class ValidationSettingsFactory : IConfigureOptions<ValidationSettings>
    {
        private readonly IConfiguration configuration;

        public ValidationSettingsFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(ValidationSettings options)
        {
            configuration.GetSection(ValidationSettings.Key).Bind(options);
        }
    }

    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            Assembly[] assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };

            services.ConfigureOptions<ValidationSettingsFactory>();
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
            services.AddAutoMapper(assemblies);

            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblies(assemblies);
                c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }

    internal sealed class ValidationSettings
    {
        public const string Key = nameof(ValidationSettings);

        public int MaxLimit { get; set; } = 100;

        public int MinimumPasswordLength { get; set; } = 8;

        public int MinimumUsernameLength { get; set; } = 3;
    }
}
