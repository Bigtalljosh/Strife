using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Strife.Blazor.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            var sub = User.Claims.First(i => i.Type == "sub").Value;
            return sub.Replace("|", "-");
        }
    }
}
