using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strife.Api.ServiceCollectionExtensions;

namespace Strife.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly bool _enableSwagger;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _enableSwagger = _configuration.GetValue<bool>("FeatureToggles:Swagger");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuth0(_configuration);
            services.AddControllers();
            services.AddHealthChecks();

            if (_enableSwagger)
            {
                var apiDetails = _configuration.GetSection("ApiDetails").Get<ApiDetails>();
                services.AddSwagger(apiDetails);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/health");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
