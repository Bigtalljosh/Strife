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

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var result = await _azureFileProvider.GetBlobs("profiles", GetUserId());

            if (result is null)
                return NotFound();

            if(result.BlobItems.Count is 0 || result.BlobItems is null)
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
