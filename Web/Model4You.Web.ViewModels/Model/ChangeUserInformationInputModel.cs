namespace Model4You.Web.ViewModels.Model
{
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ChangeUserInformationInputModel : IMapFrom<ApplicationUser>
    {
        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }
    }
}
