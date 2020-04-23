namespace Model4You.Web.ViewModels.Blog
{
    using System.Collections.Generic;

    using Ganss.XSS;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class BlogContentView : IMapFrom<BlogContent>
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ICollection<BlogComment> BlogComments { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

    }
}