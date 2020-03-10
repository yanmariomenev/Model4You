namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Location
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [MaxLength(30)]
        public string Country { get; set; }

        [MaxLength(30)]
        public string Town { get; set; }
    }
}