using Ganss.XSS;
using Model4You.Data.Models;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Blog
{
    public class BlogContentView : IMapFrom<BlogContent>
    {
        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

    }
}