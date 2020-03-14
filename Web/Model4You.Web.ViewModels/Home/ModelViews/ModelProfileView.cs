namespace Model4You.Web.ViewModels.ModelViews
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Model4You.Data.Models;
    using Model4You.Data.Models.Enums;
    using Model4You.Services.Mapping;

    public class ModelProfileView : IMapFrom<ApplicationUser> /*IHaveCustomMappings*/
    {
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        //public ModelRole ModelRole { get; set; }

        public Location Location { get; set; }

        public string ProfilePicture { get; set; }

        public ModelInformation ModelInformation { get; set; }

        //public ProfessionalInformation ProfessionalInformation { get; set; }

        //public ICollection<Blog> Blogs { get; set; }


        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<ApplicationUser, ModelProfileView>()
        //        .ForMember(x => x.UserImages,
        //            cfg => cfg.MapFrom(x => x.UserImages.FirstOrDefault()));
        //}
    }
}