namespace Model4You.Data.Models
{
    using System.Collections.Generic;

    using Model4You.Data.Common.Models;

    public class BlogContent : BaseDeletableModel<int>
    {
        public BlogContent()
        {
            //this.Images = new HashSet<UserImage>();
        }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }
        //public ICollection<UserImage> Images { get; set; }
    }
}