using System.ComponentModel.DataAnnotations;
using Model4You.Data.Models;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Model
{
    public class ChangeUserInformationInputModel : IMapFrom<ApplicationUser>
    {
        public Location Location { get; set; }

        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }
    }
}