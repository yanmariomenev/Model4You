using System.Collections.Generic;

namespace Model4You.Web.ViewModels.Model
{
    using AutoMapper;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class AlbumViewModel : IMapFrom<ApplicationUser>
    {
        // TODO Take only image urls;
        public string ProfilePicture { get; set; }

        public ICollection<UserImage> UserImages { get; set; }
    }
}