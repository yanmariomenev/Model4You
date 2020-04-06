using System;
using System.Threading.Tasks;

namespace Model4You.Services.Data.BookingService
{
    public interface IBookingService
    {
        Task<string> CreateBooking(
            string userId,
            DateTime bookingDate,
            string fullName,
            string companyName,
            string email,
            string phoneNumber,
            int? days,
            string hireDescription);

        Task<string> GetUserEmail(string userId);
    }
}