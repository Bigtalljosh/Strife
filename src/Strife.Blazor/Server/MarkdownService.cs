using Markdig;
using System.IO;
using System.Threading.Tasks;

namespace Strife.Blazor.Server
{
    public class MarkdownService
    {
        public async Task<string> GetMarkDownHtml()
        {
            var markdownFile = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "README.md"));
            var markdownHtml = Markdown.ToHtml(markdownFile);
            return markdownHtml;
        }
    }
}
