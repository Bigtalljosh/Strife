using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Strife.Blazor.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

/*
    Azure Blob storage works as follows:
    Account => Containers => Blobs
    Strife  => Profiles   => ProfilePicture
    Strife  => Media      => Video1.Mp4
    https://docs.microsoft.com/en-gb/azure/storage/blobs/storage-blobs-introduction    
*/
namespace Strife.Blazor.Server.Azure
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly string _connectionString;

        public AzureBlobService(IConfiguration config)
        {
            _connectionString = config["Azure:Blob:ConnectionString"];
        }

        /// <summary>
        /// Upload a file stream to Azure Blob Storage
        /// </summary>
        /// <param name="containerName">For example, Profiles/Media/Etc</param>
        /// <param name="blobName">Name of the file in Azure Blob storage</param>
        /// <param name="content">The file stream to upload</param>
        /// <returns>Uri of newly uploaded Blob</returns>
        public async Task<Uri> UploadPublicAsync(string containerName, string blobName, Stream content, string contentType, string fileExtension)
        {
            // Get a reference to a container 
            BlobContainerClient container = new BlobContainerClient(_connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            var blobHttpHeader = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(content, blobHttpHeader);
            await blob.SetMetadataAsync(new Dictionary<string, string>()
            {
                { "extension", fileExtension }
            });

            return blob.Uri;
        }

        /// <summary>
        /// Upload a file stream to Azure Blob Storage in a users personal space
        /// </summary>
        /// <param name="containerName">For example, Profiles/Media/Etc</param>
        /// <param name="userId">Users id</param>
        /// <param name="blobName">Name of the file in Azure Blob storage</param>
        /// <param name="content">The file stream to upload</param>
        /// <returns>Uri of newly uploaded Blob</returns>
        public async Task<Uri> UploadPrivateAsync(string containerName, string userId, string blobName, Stream content, string contentType, string fileExtension)
        {
            return await UploadPublicAsync(containerName, $"{userId}\\{blobName}", content, contentType, fileExtension);
        }

        /// <summary>
        /// List all blobs in a container
        /// </summary>
        /// <param name="containerName">Name of the container to get the blobs from</param>
        /// <returns>List of blobs in the container</returns>
        public async Task<UserItemsViewModel> ListPublicAsync(string containerName)
        {
            // Get a reference to a container 
            BlobContainerClient container = new BlobContainerClient(_connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            // List all the blobs
            var blobs = new UserItemsViewModel();

            await foreach (BlobItem blob in container.GetBlobsAsync())
            {
                blob.Metadata.TryGetValue("extension", out string extension);

                blobs.Items.Add(new UserItemViewModel
                {
                    Name = blob.Name,
                    Uri = container.Uri.ToString(),
                    FileExtension = extension ?? "Unknown"
                });
            }

            return blobs;
        }

        /// <summary>
        /// List all blobs in a users personal space
        /// </summary>
        /// <param name="containerName">Name of the container to get the blobs from</param>
        /// <param name="userId">Users id</param>
        /// <returns>List of blobs in the container</returns>
        public async Task<UserItemsViewModel> ListPrivateAsync(string containerName, string userId)
        {
            // Get a reference to a container 
            BlobContainerClient container = new BlobContainerClient(_connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            // List all the blobs
            var blobs = new UserItemsViewModel();

            await foreach (BlobItem blob in container.GetBlobsAsync(BlobTraits.All, BlobStates.All, userId))
            {
                blob.Metadata.TryGetValue("extension", out string extension);

                blobs.Items.Add(new UserItemViewModel
                {
                    Name = blob.Name,
                    Uri = $"{container.Uri}\\{blob.Name}",
                    FileExtension = extension ?? "Unknown"
                });
            }

            return blobs;
        }
    }
}
