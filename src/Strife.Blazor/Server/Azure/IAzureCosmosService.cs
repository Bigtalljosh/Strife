using Strife.Blazor.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strife.Blazor.Server.Azure
{
    public interface IAzureCosmosService
    {
        Task AddItemAsync(string containerName, UserProfile item);
        Task DeleteItemAsync(string containerName, string id);
        Task<UserProfile> GetItemAsync(string containerName, string id);
        Task<IEnumerable<UserProfile>> GetItemsAsync(string containerName, string queryString);
        Task UpdateItemAsync(string containerName, string id, UserProfile item);
    }
}