using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Strife.Blazor.Client.Auth
{
    public class Auth0ProviderOptions : OidcProviderOptions
    {
        private const string AudienceKey = "audience";
        private const string PromptKey = "prompt";

        /// <summary>
        /// Sets the audience of the application.
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Sets the prompt value of the application
        /// See https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </summary>
        public string? Prompt { get; set; }

        /// <summary>
        /// Extra Query Parameters
        /// See https://github.com/IdentityModel/oidc-client-js/blob/dev/src/OidcClient.js#L65
        /// </summary>
        [JsonPropertyName("extraQueryParams")]
        public IDictionary<string, string> ExtraQueryParams
        {
            get
            {
                var dictionary = new Dictionary<string, string>();
                if (!string.IsNullOrWhiteSpace(Audience))
                {
                    dictionary.Add(AudienceKey, Audience);
                }

                if (!string.IsNullOrWhiteSpace(Prompt))
                {
                    dictionary.Add(PromptKey, Prompt);
                }

                return dictionary;
            }
        }
    }
}
