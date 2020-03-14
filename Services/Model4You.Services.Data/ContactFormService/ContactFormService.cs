using System;
using System.Threading.Tasks;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Services.Data.ContactFormService
{
    public class ContactFormService : IContactFormService
    {
        private readonly IDeletableEntityRepository<ContractFormData> contactRepository;

        public ContactFormService(IDeletableEntityRepository<ContractFormData> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task Create(string name, string email, string subject, string message)
        {
            var contactForm = new ContractFormData
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