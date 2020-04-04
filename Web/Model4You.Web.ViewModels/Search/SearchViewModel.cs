using System.ComponentModel.DataAnnotations;
using Model4You.Data.Models;
using Model4You.Data.Models.Enums;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Search
{
    public class SearchViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        //public ModelRole ModelRole { get; set; }

        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }

        public string Url => $"/Models/Profile/{this.Id}";
    }
}