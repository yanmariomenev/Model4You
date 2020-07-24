namespace Model4You.Services.Data.Tests.Contact
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Data.Repositories;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Web.ViewModels.Administration.Dashboard;
    using Xunit;

    public class AdminContactDataServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task TakeAllUnAnswered_ShouldReturnAllUnAnsweredQuestions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactDataService(repository);

            var form = await this.CreateContactForm("Pesho", "Pesho@abv.bg", "Test", "Test",true, repository);
            var form2 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2",false, repository);
            var form3 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2", false, repository);

            var takeAll = await service.TakeAllUnAnswered<ContactFormDataView>();
            var count = takeAll.Count();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task TakeAllUnAnswered_WhenThereIsNon_ShouldReturnZero()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactDataService(repository);

            var takeAll = await service.TakeAllUnAnswered<ContactFormDataView>();
            var count = takeAll.Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task TakeAllAnswered_ShouldReturnAllUnAnsweredQuestions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactDataService(repository);

            var form = await this.CreateContactForm("Pesho", "Pesho@abv.bg", "Test", "Test", true, repository);
            var form2 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2", false, repository);
            var form3 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2", false, repository);

            var takeAll = await service.TakeAllAnswered<ContactFormDataView>();
            var count = takeAll.Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task TakeAllAnswered_WhenThereIsNon_ShouldReturnZero()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactDataService(repository);

            var takeAll = await service.TakeAllUnAnswered<ContactFormDataView>();
            var count = takeAll.Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task MoveToAnswered_ShouldMoveUnAnsweredQuestionToAnswered()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ContactFormData>(new ApplicationDbContext(options));

            var service = new ContactDataService(repository);

            var form = await this.CreateContactForm("Pesho", "Pesho@abv.bg", "Test", "Test", true, repository);
            var form2 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2", false, repository);
            var form3 = await this.CreateContactForm("Pesho2", "Pesho2@abv.bg", "Test2", "Test2", false, repository);
            var formIdToString = form2.ToString();

            await service.MoveToAnswered(formIdToString);
            var check = await repository.AllWithDeleted().Where(x => x.Id == form2 && x.IsDeleted).FirstOrDefaultAsync();

            Assert.NotNull(check);
        }

        private async Task<int> CreateContactForm(
            string name, string email, string subject, string message, bool answeredOrNot, IDeletableEntityRepository<ContactFormData> repo)
        {
            var contact = new ContactFormData
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message,
                IsDeleted = answeredOrNot,
            };
            await repo.AddAsync(contact);
            await repo.SaveChangesAsync();
            return contact.Id;
        }
    }
}
