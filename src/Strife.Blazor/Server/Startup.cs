using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strife.Blazor.Server.ApplicationBuilderExtensions;
using Strife.Blazor.Server.ServiceCollectionExtensions;
using System.Collections.Generic;

namespace Strife.Blazor.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly bool _enableSwagger;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            _enableSwagger = _configuration.GetValue<bool>("FeatureToggles:Swagger");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuth0(_configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = _configuration["Auth0:Domain"];
                    options.Audience = _configuration["Auth0:ApiIdentifier"];
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHealthChecks();

            if (_environment.IsDevelopment())
            {
                AddDevelopServices(services);
            }

            AddProjectServices(services);
        }

        private void AddProjectServices(IServiceCollection services)
        {
            services.AddSingleton(new MarkdownService());
            services.AddTransient<IAzureFileProvider, AzureFileProvider>();
        }

        private void AddDevelopServices(IServiceCollection services)
        {
            services.AddSwagger(_configuration);

            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);

                // optional - default path to view services is /listallservices - recommended to choose your own path
                config.Path = "/diservices";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseShowAllServicesMiddleware();
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.AddRobots();
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            if (_enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Strife API V1");
                });
            }
        }
    }
}
