namespace Model4You.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProfessionalInformation
    {
        // This Model is for future extension.
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [MaxLength(50)]
        public string Profession { get; set; }

        [MaxLength(300)]
        public string WebsiteOrSocialMedia { get; set; }
    }
}