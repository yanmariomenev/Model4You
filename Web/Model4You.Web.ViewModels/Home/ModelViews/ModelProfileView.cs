namespace Model4You.Web.ViewModels.ModelViews
{
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ModelProfileView : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }

        public string Url => $"/Models/Profile/{this.Id}";
    }
}
