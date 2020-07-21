namespace Strife.Blazor.Shared.Models
{
    public class UserProfile
    {
        /// <summary>
        /// This is the users ID in auth0
        /// </summary>
        public string Id { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}