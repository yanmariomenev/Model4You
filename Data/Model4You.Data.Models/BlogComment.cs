namespace Model4You.Data.Models
{
    using Model4You.Data.Common.Models;

    public class BlogComment : BaseDeletableModel<int>
    {
        public int BlogContentId { get; set; }

        public virtual BlogContent BlogContent { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }
    }
}