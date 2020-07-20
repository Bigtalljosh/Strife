using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strife.Blazor.Server.Azure;
using Strife.Blazor.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileActionsController : BaseController
    {
        private readonly IAzureBlobService _azureBlobService;

        public ProfileActionsController(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob()
        {
            try
            {
                var filename = Guid.NewGuid().ToString();
                var uri = await _azureBlobService.UploadPrivateAsync("profiles", GetUserId(), filename, Request.Body);
                return Created(uri, null);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Error saving file");
            }
        }

        [HttpGet]
        // Returning IActionResult and response codes here actually threw an issue about Blazor only supporting application/json. 
        //Not sure why yet but this is a workaround
        public async Task<ActionResult<UserItemsViewModel>> ListBlobs()
        {

            var results = await _azureBlobService.ListPrivateAsync("profiles", GetUserId());

            //if (results is null)
            //    return NotFound();

            //if (results.Count is 0)
            //    return NoContent();

            //return Ok(response);
            return results;
        }
    }
}
