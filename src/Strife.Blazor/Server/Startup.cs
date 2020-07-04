using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Strife.Blazor.Server.ServiceCollectionExtensions;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //// Add authentication services
            //services.AddAuthentication(options => {
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options => {
            //    options.Events.OnRedirectToLogin = ctx =>
            //    {
            //        // if it is an ajax/api request, don't redirect
            //        // to login page.
            //        if (!(IsAjaxRequest(ctx.Request) || IsApiRequest(ctx.Request)))
            //        {
            //            ctx.Response.Redirect(ctx.RedirectUri);
            //            return Task.CompletedTask;
            //        }
            //        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        return ctx.Response.WriteAsync("Unauthorized");
            //    };
            //})
            //.AddOpenIdConnect("Auth0", options => {
            //    // Set the authority to your Auth0 domain
            //    options.Authority = $"https://{_configuration["Auth0:Domain"]}";

            //    // Configure the Auth0 Client ID and Client Secret
            //    options.ClientId = _configuration["Auth0:ClientId"];
            //    options.ClientSecret = _configuration["Auth0:ClientSecret"];

            //    // Set response type to code
            //    options.ResponseType = "code";

            //    // Configure the scope
            //    options.Scope.Clear();
            //    options.Scope.Add("openid");

            //    // Set the callback path, so Auth0 will call back to http://localhost:44391/signin-auth0 on IIS or :5000 on Kestral
            //    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
            //    options.CallbackPath = new PathString("/signin-auth0");

            //    // Configure the Claims Issuer to be Auth0
            //    options.ClaimsIssuer = "Auth0";

            //    // Saves tokens to the AuthenticationProperties
            //    options.SaveTokens = true;

            //    options.Events = new OpenIdConnectEvents
            //    {
            //        // handle the logout redirection 
            //        OnRedirectToIdentityProviderForSignOut = (context) =>
            //        {
            //            var logoutUri = $"https://{_configuration["Auth0:Domain"]}/v2/logout?client_id={_configuration["Auth0:ClientId"]}";

            //            var postLogoutUri = context.Properties.RedirectUri;
            //            if (!string.IsNullOrEmpty(postLogoutUri))
            //            {
            //                if (postLogoutUri.StartsWith("/"))
            //                {
            //                    // transform to absolute
            //                    var request = context.Request;
            //                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
            //                }
            //                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
            //            }

            //            context.Response.Redirect(logoutUri);
            //            context.HandleResponse();

            //            return Task.CompletedTask;
            //        }
            //    };
            //})
            //.AddJwtBearer(options => {
            //    options.Authority = $"https://{_configuration["Auth0:Domain"]}";
            //    options.Audience = _configuration["Auth0:ApiIdentifier"];
            //});

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHealthChecks();

            if (_environment.IsDevelopment())
            {
                services.AddSwagger(_configuration);
            }

            AddProjectServices(services);
        }

        private void AddProjectServices(IServiceCollection services)
        {
            services.AddSingleton(new MarkdownService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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

        private static bool IsAjaxRequest(HttpRequest request)
        {
            var query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        private static bool IsApiRequest(HttpRequest request)
        {
            return request.Path.StartsWithSegments(new PathString("/api"));
        }
    }
}
