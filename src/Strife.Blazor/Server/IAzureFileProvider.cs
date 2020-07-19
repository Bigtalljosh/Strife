using Strife.Blazor.Shared.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace Strife.Blazor.Server
{
    public interface IAzureFileProvider
    {
        Task<UserItemsViewModel> GetBlobs(string blobContainer, string directoryName);
        Task<string> UploadBlob(string blobContainer, Stream stream, string directoryName);
    }
}