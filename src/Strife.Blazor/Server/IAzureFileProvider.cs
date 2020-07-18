using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Strife.Blazor.Server
{
    public interface IAzureFileProvider
    {
        Task<List<IListBlobItem>> GetBlobs(string blobContainer);
        Task<string> UploadBlob(string blobContainer, Stream stream, string directoryName);
    }
}