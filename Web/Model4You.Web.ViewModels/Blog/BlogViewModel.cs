using System;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Blog
{
    public class BlogViewModel : IMapFrom<Data.Models.Blog>
    {
        public string Title { get; set; }

        public string UserId { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}