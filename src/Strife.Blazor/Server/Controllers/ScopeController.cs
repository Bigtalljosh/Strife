using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strife.Blazor.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScopeController : ControllerBase
    {
        [HttpGet("public")]
        public async Task<IActionResult> PublicAsync()
        {
            string tokenInfo = "";

            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                // if you need to check the Access Token expiration time, use this value
                // provided on the authorization response and stored.
                // do not attempt to inspect/decode the access token
                DateTime accessTokenExpiresAt = DateTime.Parse(
                    await HttpContext.GetTokenAsync("expires_at"),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

                string idToken = await HttpContext.GetTokenAsync("id_token");

                // Now you can use them. For more info on when and how to use the
                // Access Token and ID Token, see https://auth0.com/docs/tokens

                tokenInfo = $"Access Token: {accessToken} | ID Token: {idToken} | Expires At: {accessTokenExpiresAt}";
            }

            return Ok(new
            {
                Message = $"Hello from a public endpoint! You don't need to be authenticated to see this. But if you are you should see some data here: {tokenInfo}"
            });
        }

        [HttpGet("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }

        [HttpGet("private-scoped")]
        [Authorize("read:test")]
        public IActionResult Scoped()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated and have a scope of test:scope to see this."
            });
        }

        // This is a helper action. It allows you to easily view all the claims of the token.
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            var tokenInfo = new List<Token>();

            var scopes = User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                });

            foreach (var scope in scopes)
            {
                tokenInfo.Add(new Token { Type = scope.Type, Value = scope.Value });
            }

            if(tokenInfo.Count == 0)
            {
                tokenInfo.Add(new Token { Type = "test", Value = "This is a test response as you had no token on the request" });
            }

            return Ok(tokenInfo);
        }
    }
}
