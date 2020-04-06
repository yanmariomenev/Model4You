using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Services.Data.BookingService
{
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

        public async Task<string> GetUserEmail(string userId)
        {
            var userEmail = await this.userRepository.All().Where(x => x.Id == userId)
                .Select(x => x.Email).FirstOrDefaultAsync();

            return userEmail;
        }
    }
}