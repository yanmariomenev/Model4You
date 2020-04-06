using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Common;
using Model4You.Services.Data.BookingService;
using Model4You.Services.Messaging;
using Model4You.Web.ViewModels.Booking;

namespace Model4You.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;
        private readonly IEmailSender emailSender;

        public BookingController(IBookingService bookingService, IEmailSender emailSender)
        {
            this.bookingService = bookingService;
            this.emailSender = emailSender;
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

            var status = await this.bookingService.CreateBooking(
                id,
                input.BookingDate,
                input.FullName,
                input.CompanyName,
                input.Email,
                input.PhoneNumber, input.Days, input.HireDescription);

            // TODO Test this because Sendgrid suspended the account...
            var bookedUserEmail = await this.bookingService.GetUserEmail(id);

            // TODO Build an html email response if there is time;
            var emailContentBuilder = new StringBuilder();
            emailContentBuilder.AppendLine($"Company name - {input.CompanyName}");
            emailContentBuilder.AppendLine($"Phone number - {input.PhoneNumber}");
            emailContentBuilder.AppendLine($"Approximate days for hire - {input.Days}");
            emailContentBuilder.AppendLine($"Description : {input.HireDescription}");
            await this.emailSender.SendEmailAsync(
                input.Email,
                input.FullName,
                bookedUserEmail,
                GlobalConstants.BookingRequest,
                emailContentBuilder.ToString()
                );
            //await this.emailSender.SendEmailAsync(
            //    input.e,
            //    model.Name,
            //    GlobalConstants.SystemEmail,
            //    model.Subject,
            //    model.Message);
            return this.RedirectToAction("Result", new { result = status });
        }

        public IActionResult Result(string result)
        {
            var viewModel = new ResultViewModel
            {
                Status = result,
            };

            return this.View(viewModel);
        }
    }
}