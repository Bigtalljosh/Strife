using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Strife.Blazor.Client.Auth;
using Strife.Blazor.Shared.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Strife.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            await LoadAppSettingsAsync(builder);

            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton(new BaseAddress(builder.HostEnvironment.BaseAddress));

            var auth0Config = builder.Configuration.GetSection("Auth0").Get<Auth0Config>();
            builder.Services.AddSingleton(auth0Config);

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // We use these to pass the token along with the request
            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddHttpClient("ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                                            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = auth0Config.Authority;
                options.ProviderOptions.ClientId = auth0Config.ClientId;
                options.ProviderOptions.ResponseType = "token id_token";
                options.ProviderOptions.PostLogoutRedirectUri = "/index";
            });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddHeadElementHelper();

            await builder.Build().RunAsync();
        }

        /// <summary>
        /// Load appsettings.json from the  wwwroot folder
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static async Task LoadAppSettingsAsync(WebAssemblyHostBuilder builder)
        {
            var client = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            using var response = await client.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();
            builder.Configuration.AddJsonStream(stream);
        }
    }
}
