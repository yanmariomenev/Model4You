namespace Model4You.Web.ViewModels.Blog
{
    using System;

    using AutoMapper;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class BlogViewModel : IMapFrom<Blog>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url => $"/Blog/BlogArticle/{this.Id}";

    }
}