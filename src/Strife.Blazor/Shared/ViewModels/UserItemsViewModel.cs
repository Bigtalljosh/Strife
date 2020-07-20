using System.Collections.Generic;

namespace Strife.Blazor.Shared.ViewModels
{
    public class UserItemsViewModel
    {
        public UserItemsViewModel()
        {
            Items = new List<UserItemViewModel>();
        }

        public List<UserItemViewModel> Items { get; set; }
    }

    public class UserItemViewModel
    {
        public string Name { get; set; }
        public string Uri { get; set; }
    }
}
