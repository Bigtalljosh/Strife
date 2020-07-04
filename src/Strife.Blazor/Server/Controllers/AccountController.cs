using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strife.Blazor.Shared.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            //{
            //    // Indicate here where Auth0 should redirect the user after a logout.
            //    // Note that the resulting absolute Uri must be whitelisted in the 
            //    // **Allowed Logout URLs** settings for the client.
            //    RedirectUri = Url.Action("Index", "Home")
            //});
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View(new UserProfileViewModel()
            {
                Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
                Country = User.Claims.FirstOrDefault(c => c.Type == "country")?.Value
            });
        }


        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}