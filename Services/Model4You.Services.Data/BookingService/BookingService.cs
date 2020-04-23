namespace Model4You.Services.Data.BookingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class BookingService : IBookingService
    {
        private readonly IDeletableEntityRepository<Booking> bookingRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public BookingService(
            IDeletableEntityRepository<Booking> bookingRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.bookingRepository = bookingRepository;
            this.userRepository = userRepository;
        }

        public async Task<string> CreateBooking(
            string userId,
            DateTime bookingDate,
            string fullName,
            string companyName,
            string email,
            string phoneNumber,
            int? days,
            string hireDescription)
        {
            var userExist = await this.userRepository
                .All()
                .AnyAsync(x => x.Id == userId);

            if (!userExist)
            {
                return "Booking failed! Please try again.";
            }

            var booking = new Booking
            {
                UserId = userId,
                BookingDate = bookingDate,
                FullName = fullName,
                CompanyName = companyName,
                Email = email,
                PhoneNumber = phoneNumber,
                Days = days,
                HireDescription = hireDescription,
            };

            await this.bookingRepository.AddAsync(booking);
            await this.bookingRepository.SaveChangesAsync();

            return "Booking was successful! Please wait for the model to contact you back";
        }

        // This should not be here but apparently needed it when i was doing the booking.
        public async Task<string> GetUserEmail(string userId)
        {
            var userEmail = await this.userRepository.All().Where(x => x.Id == userId)
                .Select(x => x.Email).FirstOrDefaultAsync();

            return userEmail;
        }

        public async Task<int> GetPagesCount(int perPage, string userId)
        {
            var profiles =
                await this.bookingRepository
                    .All()
                    .Where(x => x.UserId == userId && !x.IsDeleted)
                    .CountAsync();
            var count = (int)Math.Ceiling(profiles / (decimal)perPage);

            return count;
        }

        public async Task<IEnumerable<T>> TakeAllBookingsForCurrentUser<T>(string userId, int page, int perPage)
        {
            var bookings = this.bookingRepository.All()
                .Where(x => !x.IsDeleted && x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(perPage * (page - 1))
                .Take(perPage);
            return await bookings.To<T>().ToListAsync();
        }

        public async Task<T> GetBookingById<T>(string id, string userId)
        {
            var booking = this.bookingRepository.All().Where(x => x.Id == id && x.UserId == userId);

            return await booking.To<T>().FirstOrDefaultAsync();
        }

        public async Task DeleteBooking(string id, string userId)
        {
            var deleteBooking =
                await this.bookingRepository.All()
                    .Where(x => x.Id == id && x.UserId == userId)
                    .FirstOrDefaultAsync();
            this.bookingRepository.Delete(deleteBooking);
            await this.bookingRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> TakeAllDeletedBookings<T>(string id)
        {
            var archive = this.bookingRepository.AllWithDeleted().Where(x => x.UserId == id && x.IsDeleted);

            return await archive.To<T>().ToListAsync();
        }

        public async Task UnDeleteBooking(string id)
        {
            var unDelete = await this.bookingRepository.AllWithDeleted().Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            this.bookingRepository.Undelete(unDelete);
            await this.bookingRepository.SaveChangesAsync();
        }
    }
}