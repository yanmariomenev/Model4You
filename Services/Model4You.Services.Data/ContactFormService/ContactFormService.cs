using System;
using System.Threading.Tasks;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Services.Data.ContactFormService
{
    public class ContactFormService : IContactFormService
    {
        private readonly IDeletableEntityRepository<ContactFormData> contactRepository;

        public ContactFormService(IDeletableEntityRepository<ContactFormData> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task Create(string name, string email, string subject, string message)
        {
            var contactForm = new ContactFormData
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message,
            };

            await this.contactRepository.AddAsync(contactForm);
            await this.contactRepository.SaveChangesAsync();
        }


    }
}