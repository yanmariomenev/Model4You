using Model4You.Data.Common.Models;

namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Models.Enums;

    public class ModelInformation : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Range(2, 110)]
        public int Age { get; set; }

        public Gender Gender { get; set; }

        public double Height { get; set; }

        public string ModelType { get; set; }

        public double Bust { get; set; }

        public double Waist { get; set; }

        public double Hips { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string Nationality { get; set; }

        public Ethnicity Ethnicity { get; set; }

        [MaxLength(30)]
        public string Country { get; set; }

        [MaxLength(30)]
        public string Town { get; set; }
    }
}