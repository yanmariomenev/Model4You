namespace Model4You.Web.ViewModels.Booking
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class BookingInputModel : IMapFrom<Booking>
    {
        //[Required]
        //public string UserId { get; set; }

        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(60)]
        public string FullName { get; set; }

        [MaxLength(70)]
        public string CompanyName { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        public int? Days { get; set; }

        [Required]
        [MaxLength(600)]
        public string HireDescription { get; set; }
    }
}