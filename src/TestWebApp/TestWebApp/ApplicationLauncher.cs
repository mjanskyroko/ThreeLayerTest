namespace TestWebApp
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    internal static class HostEnvironment
    {
        public const string Development = nameof(Development);
        public const string Variable = "ASPNETCORE_ENVIRONMENT";
    }

    internal static class ApplicationLauncher
    {
        public static async Task RunAsync<TStartup>(string[] args)
            where TStartup : class
        {
            var builder = new ConfigurationBuilder();

            BuildConfiguration<TStartup>(builder, args);

            await CreateHostBuilder<TStartup>(args).Build().RunAsync();

        }

        private static IHostBuilder CreateHostBuilder<TStartup>(string[] args)
            where TStartup : class
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<TStartup>());
        }

        private static void BuildConfiguration<TStartup>(IConfigurationBuilder builder, string[] args)
            where TStartup : class
        {
            var environment = Environment.GetEnvironmentVariable(HostEnvironment.Variable);

            var isDevelopment = string.IsNullOrWhiteSpace(environment)
                || string.Equals(environment, HostEnvironment.Development, StringComparison.OrdinalIgnoreCase);

            builder
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    path: "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile(
                    path: $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables();

            if (isDevelopment)
            {
                builder.AddUserSecrets<TStartup>();
            }

            builder.AddCommandLine(args);
        }
    }

}
