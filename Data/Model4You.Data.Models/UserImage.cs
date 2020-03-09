namespace Model4You.Data.Models
{
    using Model4You.Data.Common.Models;

    public class UserImage : BaseDeletableModel<int>
    {
        public string ImageUrl { get; set; }
    }
}