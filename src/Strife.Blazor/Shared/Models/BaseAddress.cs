namespace Strife.Blazor.Shared.Models
{
    public class BaseAddress
    {
        public string Url { get; }

        public BaseAddress(string url)
        {
            Url = url;
        }
    }
}
