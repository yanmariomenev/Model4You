using System;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Inbox
{
    public class InboxViewModel : IMapFrom<Data.Models.Booking>
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string HireDescription { get; set; }

    }
}