using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Repositories;
using Model4You.Services.Data.AdminServices;
using Model4You.Web.ViewModels.Inbox;
using Xunit;

namespace Model4You.Services.Data.Tests.Booking
{
    public class BookingServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task CreateBooking_ShouldCreateBookingForTheModel()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            var booking = await service.CreateBooking(
                user1,
                DateTime.UtcNow,
                "TestName",
                "TestCompany",
                "test@abv.bg",
                "059593",
                3,
                "descriptionTest");

            var success = "Booking was successful! Please wait for the model to contact you back";
            var getBooking = await bookingRepository
                .All()
                .Where(x => x.UserId == user1)
                .FirstOrDefaultAsync();

            Assert.Equal(success, booking);
            Assert.NotNull(getBooking);
        }

        [Fact]
        public async Task GetUserEmail_ShouldReturnUserEmail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);

            var getUserEmail = await service.GetUserEmail(user1);


            Assert.Equal("pesho@abv.bg", getUserEmail);
        }

        [Fact]
        public async Task CreateBooking_WithNotValidUser_ShouldReturnFailMessage()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            var fakeUserId = "FakeUser6006";
            var booking = await service.CreateBooking(
                fakeUserId,
                DateTime.UtcNow,
                "TestName",
                "TestCompany",
                "test@abv.bg",
                "059593",
                3,
                "descriptionTest");

            var failedNoUser = "Booking failed! Please try again.";
            var getBooking = await bookingRepository
                .All()
                .Where(x => x.UserId == failedNoUser)
                .FirstOrDefaultAsync();

            Assert.Equal(failedNoUser, booking);
            Assert.Null(getBooking);
        }

        [Fact]
        public async Task TakeAllBookingsForCurrentUser_DependingOnPerPage_ShouldReturnAllBookingsForTheUser_DependingOnPerPageNumber()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);

            for (int i = 0; i < 18; i++)
            {
                await service.CreateBooking(
                    user1,
                    DateTime.UtcNow,
                    $"TestName{i}",
                    "TestCompany",
                    $"test{i}@abv.bg",
                    $"059593{i}",
                    1,
                    $"descriptionTest{i}");
            }

            var perPage = 10;
            var pagesCount = await service.GetPagesCount(perPage, user1);
            var takeAllBookings = await service
                .TakeAllBookingsForCurrentUser<InboxViewModel>(user1, 1, perPage);
            var takeAllBookingsTwo = await service
                .TakeAllBookingsForCurrentUser<InboxViewModel>(user1, 2, perPage);
            var bookingsReturned = takeAllBookings.Count();
            var bookingsReturnedPageTwo = takeAllBookingsTwo.Count();

            // First page should return 10 and second 8 for overall 18 blogs
            Assert.Equal(10, bookingsReturned);
            Assert.Equal(8, bookingsReturnedPageTwo);
            Assert.Equal(2, pagesCount);

        }

        [Fact]
        public async Task TakeAllBookingsForCurrentUser_WithNoBookings_ShouldReturnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);

            var perPage = 10;
            var pagesCount = await service.GetPagesCount(perPage, user1);
            var takeAllBookings = await service
                .TakeAllBookingsForCurrentUser<InboxViewModel>(user1, 1, perPage);
            
            Assert.Empty(takeAllBookings);
        }

        [Fact]
        public async Task GetBookingById_ShouldReturnTheBookingByUserIdAndBookingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            var booking = await service.CreateBooking(
                user1,
                DateTime.UtcNow,
                "TestName",
                "TestCompany",
                "test@abv.bg",
                "059593",
                1,
                "descriptionTest");
            for (int i = 0; i < 3; i++)
            {
                await service.CreateBooking(
                    user1,
                    DateTime.UtcNow,
                    $"TestName{i}",
                    "TestCompany",
                    $"test{i}@abv.bg",
                    $"059593{i}",
                    1,
                    $"descriptionTest{i}");
            }

            var getBookingId = await bookingRepository.All()
                .Where(x => x.UserId == user1 && x.HireDescription == "descriptionTest")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            var getBookingById = await service.GetBookingById<InboxViewModel>(getBookingId, user1);

            Assert.Equal("TestName", getBookingById.FullName);
            Assert.Equal("test@abv.bg", getBookingById.Email);
        }

        [Fact]
        public async Task GetBookingById_WithWrongInformation_ShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var bookingRepository = new EfDeletableEntityRepository<Model4You.Data.Models.Booking>(new ApplicationDbContext(options));

            var service = new BookingService.BookingService(bookingRepository, userRepository);
            var user1 = await this.CreateUserWithNoInformationAsync
                ("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            for (int i = 0; i < 2; i++)
            {
                await service.CreateBooking(
                    user1,
                    DateTime.UtcNow,
                    $"TestName{i}",
                    "TestCompany",
                    $"test{i}@abv.bg",
                    $"059593{i}",
                    1,
                    $"descriptionTest{i}");
            }
            var getBookingById = await service.GetBookingById<InboxViewModel>("FakeUser6006", user1);

            Assert.Null(getBookingById);
        }

        private async Task<string> CreateUserWithNoInformationAsync
            (string email, string name, string lastName, IDeletableEntityRepository<ApplicationUser> repo)
        {
            var user = new ApplicationUser()
            {
                FirstName = name,
                LastName = lastName,
                Email = email,
                UserName = email,
            };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();
            return user.Id;
        }
    }
}