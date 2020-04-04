namespace Model4You.Web.ViewModels.Search
{
    using System.ComponentModel.DataAnnotations;

    public class SearchInputModel
    {
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Range(12, 74)]
        public int Age { get; set; }

        public int To { get; set; }
    }
}