using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StrifeInfoController : ControllerBase
    {
        private readonly MarkdownService _markdownService;
        public StrifeInfoController(MarkdownService markdownService)
        {
            _markdownService = markdownService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var md = await _markdownService.GetMarkDownHtml();
            return Ok(md);
        }
    }
}
