using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileActionsController : BaseController
    {
        private readonly IAzureFileProvider _azureFileProvider;

        public ProfileActionsController(IAzureFileProvider azureFileProvider)
        {
            _azureFileProvider = azureFileProvider;
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePicture()
        {
            try
            {
                var uri = await _azureFileProvider.UploadBlob("profiles", Request.Body, GetUserId());
                return Created(uri, null);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Error saving file");
            }
        }
    }
}
