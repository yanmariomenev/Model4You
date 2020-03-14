namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Models.Enums;

    public class ModelInformation
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Range(2, 110)]
        public int Age { get; set; }

        public bool Gender { get; set; }

        public double Height { get; set; }

        public string ModelType { get; set; }

        public string BodyType { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }

        [Required]
        public string Nationality { get; set; }

        public Ethnicity Ethnicity { get; set; }
    }
}