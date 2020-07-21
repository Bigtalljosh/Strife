using Microsoft.Azure.Cosmos;
using Strife.Blazor.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Azure
{
    public class AzureCosmosService : IAzureCosmosService
    {
        private readonly CosmosClient _client;
        private Container _container;
        private readonly string _databaseName;

        public AzureCosmosService(CosmosClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }

        public async Task AddItemAsync(string containerName, UserProfile item)
        {
            await GetContainer(containerName);
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string containerName, string id)
        {
            await GetContainer(containerName);
            await _container.DeleteItemAsync<UserProfile>(id, new PartitionKey(id));
        }

        public async Task<UserProfile> GetItemAsync(string containerName, string id)
        {
            try
            {
                await GetContainer(containerName);
                ItemResponse<UserProfile> response = await _container.ReadItemAsync<UserProfile>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode is HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserProfile>> GetItemsAsync(string containerName, string queryString)
        {
            await GetContainer(containerName);
            var query = _container.GetItemQueryIterator<UserProfile>(new QueryDefinition(queryString));
            List<UserProfile> results = new List<UserProfile>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string containerName, string id, UserProfile item)
        {
            await GetContainer(containerName);
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }

        private async Task GetContainer(string containerName)
        {
            try
            {
                var database = await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
                await database.Database.CreateContainerIfNotExistsAsync(containerName, "/keypath");
                _container = _client.GetContainer(_databaseName, containerName);
            }
            catch (CosmosException ex) when (ex.StatusCode is HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Cannot create container {containerName}: ", ex);
                throw ex;
            }
            catch (CosmosException ex) when (ex.StatusCode is HttpStatusCode.Forbidden)
            {
                Console.WriteLine($"Cannot create container {containerName}, Exceeded Quota.");
                throw ex;
            }
            catch (CosmosException ex) when (ex.StatusCode is HttpStatusCode.Conflict)
            {
                Console.WriteLine($"Cannot create container {containerName}, Conflict.");
                throw ex;
            }
        }
    }
}
