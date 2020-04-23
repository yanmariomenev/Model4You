namespace Model4You.Web.ViewModels.Home.ContactView
{
    using System.ComponentModel.DataAnnotations;

    public class ContactInputModel
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(255)]
        public string Message { get; set; }
    }
}