namespace Model4You.Data.Models
{
    using Model4You.Data.Common.Models;

    public class UserImage : BaseDeletableModel<int>
    {
        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}