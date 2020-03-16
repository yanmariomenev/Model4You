using Model4You.Data.Common.Models;

namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Location : BaseModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [MaxLength(30)]
        public string Country { get; set; }

        [MaxLength(30)]
        public string Town { get; set; }
    }
}