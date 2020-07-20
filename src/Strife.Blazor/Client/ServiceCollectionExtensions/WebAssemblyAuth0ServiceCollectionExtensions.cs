using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Strife.Blazor.Client.Auth;
using System;

namespace Strife.Blazor.Client.ServiceCollectionExtensions
{
    public static class WebAssemblyAuth0ServiceCollectionExtensions
    {
        public static IRemoteAuthenticationBuilder<RemoteAuthenticationState, RemoteUserAccount> AddAuth0Authentication(this IServiceCollection services, Action<RemoteAuthenticationOptions<Auth0ProviderOptions>> configure)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<RemoteAuthenticationOptions<Auth0ProviderOptions>>, DefaultAuth0ProviderOptionsConfiguration>());

            return services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, Auth0ProviderOptions>(configure);
        }
    }
}
