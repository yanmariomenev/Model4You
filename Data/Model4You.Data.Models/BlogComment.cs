namespace Model4You.Data.Models
{
    using Model4You.Data.Common.Models;

    public class BlogComment : BaseDeletableModel<int>
    {
        public int BlogContentId { get; set; }

        public BlogContent BlogContent { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Content { get; set; }
    }
}