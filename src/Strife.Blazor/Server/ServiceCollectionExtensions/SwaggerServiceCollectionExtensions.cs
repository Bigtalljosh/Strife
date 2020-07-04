using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Strife.Blazor.Server.ServiceCollectionExtensions
{
    internal static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            ApiDetails apiDetails = new ApiDetails();
            configuration.GetSection("ApiDetails").Bind(apiDetails);

            services.AddSwaggerGen(options =>
            {
                var info = new OpenApiInfo
                {
                    Title = $"{apiDetails.Title} - {apiDetails.Owners}",
                    Version = "v1",
                    Description = $"### {apiDetails.Description}\n" +
                              $"### [{apiDetails.Title} Git Repository]({apiDetails.GitRepoUrl})"
                };

                options.SwaggerDoc("v1", info);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
