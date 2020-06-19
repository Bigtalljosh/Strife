using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strife.Blazor.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Strife.Blazor.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScopeController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
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
