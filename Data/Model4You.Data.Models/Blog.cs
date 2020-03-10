namespace Model4You.Data.Models
{
    using System.Collections.Generic;

    using Model4You.Data.Common.Models;

    public class Blog : BaseDeletableModel<int>
    {
        public Blog()
        {
            this.BlogContents = new HashSet<BlogContent>();
        }

        public string Title { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Count { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<BlogContent> BlogContents { get; set; }
    }
}