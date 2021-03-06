﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strife.Blazor.Server.Azure;
using System;
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
                var contentType = Request.Headers["filecontenttype"];
                var fileExtension = Request.Headers["fileextension"];
                var uri = await _azureBlobService.UploadPrivateAsync("profiles", GetUserId(), filename, Request.Body, contentType, fileExtension);
                return Created(uri, null);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Error saving file");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListBlobs()
        {
            var results = await _azureBlobService.ListPrivateAsync("profiles", GetUserId());

            if (results is null)
                return NotFound();

            if (results.Items.Count is 0)
                return NoContent();

            return Ok(results);
        }
    }
}
