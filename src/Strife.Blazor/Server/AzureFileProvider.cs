using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Strife.Blazor.Shared.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Strife.Blazor.Server
{
    public class AzureFileProvider : IAzureFileProvider
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _blobClient;

        public AzureFileProvider(IConfiguration config)
        {
            StorageCredentials storageCredentials = new StorageCredentials(config["Azure:StorageCredentials:AccountName"], config["Azure:StorageCredentials:AccountKey"]);
            _cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<string> UploadBlob(string blobContainer, Stream stream, string directoryName)
        {
            string fileName = Guid.NewGuid().ToString();

            CloudBlobContainer container = _blobClient.GetContainerReference(blobContainer);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(directoryName + @"\" + fileName);

            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri.ToString();
        }

        public async Task<UserItemsViewModel> GetBlobs(string blobContainer, string directoryName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(blobContainer);
            
            if (container is null)
                return null;

            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            BlobContinuationToken continuationToken = null;
            var userItemsViewModel = new UserItemsViewModel();

            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                userItemsViewModel.BlobItems.AddRange(response.Results);
            }
            while (continuationToken != null);

            return userItemsViewModel;
        }
    }
}
