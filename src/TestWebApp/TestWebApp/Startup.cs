namespace TestWebApp
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TestWebApp.Infrastructure.Database.Mssql;
    using TestWebApp.Application;
    using TestWebApp.Infrastructure.Services;
    using Microsoft.Extensions.Hosting;

    internal sealed class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public MssqlAdapterSettings MssqlSettings =>
            this.Configuration
            .GetSection(MssqlAdapterSettings.Key)
            .Get<MssqlAdapterSettings>() ?? throw new ArgumentNullException(nameof(this.MssqlSettings));


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMssqlDatabaseLayer(this.MssqlSettings);
            services.AddApplicationLayer();
            services.AddServices();
            
            
            // TODO: add presentation layer
            //services.AddPresentationConfiguration(this.Environment);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!this.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.MigrateMssqlDb(); // Does migration

            app.UseHttpsRedirection();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
