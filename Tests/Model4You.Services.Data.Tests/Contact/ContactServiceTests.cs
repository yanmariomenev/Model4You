namespace Model4You.Services.Data.Tests.Contact
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Data.Repositories;
    using Xunit;

    public class ContactServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task Create_ShouldCreateForm()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactFormService.ContactFormService(repository);

            await service.Create("Pesho2", "Pesho2@abv.bg", "Test2", "Test2");
            var count = await repository.All().CountAsync();

            Assert.Equal(1, count);

        }

        private async Task<int> CreateContactForm
            (string name, string email, string subject, string message, IDeletableEntityRepository<ContactFormData> repo)
        {
            var contact = new ContactFormData
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message,
            };
            await repo.AddAsync(contact);
            await repo.SaveChangesAsync();
            return contact.Id;
        }
    }
}
