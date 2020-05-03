namespace Model4You.Services.Data.BookingService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        Task<int> GetPagesCount(int perPage, string userId);

        Task<IEnumerable<T>> TakeAllBookingsForCurrentUser<T>(string userId, int page, int perPage);

        Task<T> GetBookingById<T>(string id, string userId);

        Task DeleteBooking(string id, string userId);

        Task CancelBooking(string id, string userId);

        Task<IEnumerable<T>> TakeAllDeletedBookings<T>(string id);

        Task UnDeleteBooking(string id);
    }
}