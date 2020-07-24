namespace Model4You.Data.Models
{
    using System.Collections.Generic;

    using Model4You.Data.Common.Models;

    public class BlogContent : BaseDeletableModel<int>
    {
        public BlogContent()
        {
            this.BlogComments = new HashSet<BlogComment>();
        }

        // Future implementation ViewCount.
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<BlogComment> BlogComments { get; set; }
    }
}
