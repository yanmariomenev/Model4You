namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProfessionalInformation
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Profession { get; set; }

        [MaxLength(300)]
        public string WebsiteOrSocialMedia { get; set; }
    }
}