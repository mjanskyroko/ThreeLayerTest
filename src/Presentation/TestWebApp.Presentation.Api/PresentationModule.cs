namespace TestWebApp.Presentation.Api
{
    using System;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Serialization;
    using TestWebApp.Application.Internal.Exceptions;

    public static class PresentationModule
    {
        public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            Action<ApiVersioningOptions> versioningOptions = options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            };

            Action<ApiExplorerOptions> explorerOptions = options =>
            {
                // Formats version as "'v'[major][.minor][-status]".
                options.GroupNameFormat = "'v'VV";
                options.SubstituteApiVersionInUrl = true;
            };

            services.AddApiVersioning(versioningOptions).AddVersionedApiExplorer(explorerOptions);

            services.AddProblemDetails(opt =>
            {
                opt.Map<ServiceValidationException>(ex =>
                    new ValidationProblemDetails(ex.Errors)
                    {
                        Title = ex.Message,
                        Detail = ex.Detail,
                        Status = StatusCodes.Status400BadRequest
                    });
            });
            return services;
        }
    }
}
