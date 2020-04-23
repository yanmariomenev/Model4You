namespace Model4You.Web.ViewModels.Inbox
{
    using System;

    using Model4You.Services.Mapping;

    public class InboxViewModel : IMapFrom<Data.Models.Booking>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string HireDescription { get; set; }

    }
}