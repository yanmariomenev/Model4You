using Model4You.Common;
using Model4You.Services.Messaging;

namespace Model4You.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.BookingService;
    using Model4You.Web.ViewModels.Inbox;

    public class InboxController : Controller
    {
        private const int PerPage = 10;
        private readonly IBookingService bookService;

        public InboxController(IBookingService bookService)
        {
            this.bookService = bookService;
        }

        [Authorize]
        public async Task<IActionResult> Bookings(int page = 1, int perPage = PerPage)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pagesCount = await this.bookService.GetPagesCount(perPage, userId);
            var viewModel = new InboxBookingViewModel
            {
                InboxViewModels = await this.bookService.TakeAllBookingsForCurrentUser<InboxViewModel>(userId, page, perPage),
                CurrentPage = page,
                PagesCount = pagesCount,
            };
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.RedirectToAction(nameof(this.Bookings));
            }

            await this.bookService.DeleteBooking(id, userId);
            return this.RedirectToAction(nameof(this.Bookings));
        }

        [Authorize]
        public async Task<IActionResult> ViewBooking(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = await this.bookService.GetBookingById<InboxViewModel>(id, userId);

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Archive()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new InboxBookingViewModel
            {
                InboxViewModels = await this.bookService.TakeAllDeletedBookings<InboxViewModel>(userId),
            };
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Cancel(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.bookService.CancelBooking(id, userId);

            return this.RedirectToAction(nameof(this.Bookings));
        }

        [Authorize]
        public async Task<IActionResult> Return(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.RedirectToAction(nameof(this.Archive));
            }

            await this.bookService.UnDeleteBooking(id);
            return this.RedirectToAction(nameof(this.Bookings));
        }
    }
}