using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Blog
{
    public class CommentInputModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public int BlogContentId { get; set; }

        public int BlogId { get; set; }
    }
}