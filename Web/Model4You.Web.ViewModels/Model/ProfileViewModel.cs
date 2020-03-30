namespace Model4You.Web.ViewModels.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        //public ModelRole ModelRole { get; set; }

        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }

        public ICollection<UserImage> UserImages { get; set; }

        //public ICollection<UserImage> UserImages { get; set; }

    }
}