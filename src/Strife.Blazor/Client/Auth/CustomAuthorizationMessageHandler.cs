using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Strife.Blazor.Client.Auth
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost" },
                scopes: new[] { "profile:write" });
        }
    }
}
