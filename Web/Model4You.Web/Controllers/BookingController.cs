using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Services.Data.BookingService;
using Model4You.Web.ViewModels.Booking;

namespace Model4You.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        public IActionResult Create(string id)
        {
            // TODO GET USER NAME AND DISPLAY IT
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, BookingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result= await this.bookingService.CreateBooking(id, input.BookingDate,
                input.FullName, input.CompanyName,
                input.PhoneNumber, input.Days, input.HireDescription);

            return this.Redirect("/");
        }
    }
}