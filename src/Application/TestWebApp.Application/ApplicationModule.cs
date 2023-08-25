namespace TestWebApp.Application
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using TestWebApp.Application.Internal.Behaviors;

    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddApplicationConfiguration(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblies(assemblies);
                c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}
