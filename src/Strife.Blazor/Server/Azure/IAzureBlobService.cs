using Strife.Blazor.Shared.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Azure
{
    public interface IAzureBlobService
    {
        Task<UserItemsViewModel> ListPrivateAsync(string containerName, string userId);
        Task<UserItemsViewModel> ListPublicAsync(string containerName);
        Task<Uri> UploadPrivateAsync(string containerName, string userId, string blobName, Stream content);
        Task<Uri> UploadPublicAsync(string containerName, string blobName, Stream content);
    }
}