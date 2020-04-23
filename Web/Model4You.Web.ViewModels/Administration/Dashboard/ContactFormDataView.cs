namespace Model4You.Web.ViewModels.Administration.Dashboard
{
    using System;

    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ContactFormDataView : IMapFrom<ContactFormData>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}