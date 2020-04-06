namespace Model4You.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Common.Models;

    public class Booking : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(60)]
        public string FullName { get; set; }

        [MaxLength(70)]
        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public int? Days { get; set; }

        [Required]
        [MaxLength(600)]
        public string HireDescription { get; set; }

    }
}