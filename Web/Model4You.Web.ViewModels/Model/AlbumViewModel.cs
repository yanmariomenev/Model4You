namespace Model4You.Web.ViewModels.Model
{
    using System.Collections.Generic;

    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class AlbumViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string ProfilePicture { get; set; }

        public ICollection<UserImage> UserImages { get; set; }
    }
}
