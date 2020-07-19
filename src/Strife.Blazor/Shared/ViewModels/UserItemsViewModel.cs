using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

namespace Strife.Blazor.Shared.ViewModels
{
    public class UserItemsViewModel
    {
        public UserItemsViewModel()
        {
            BlobItems = new List<IListBlobItem>();
        }

        public List<IListBlobItem> BlobItems { get; set; }
    }
}
