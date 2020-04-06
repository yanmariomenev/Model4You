using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Services.Data.BookingService;
using Model4You.Web.ViewModels.Inbox;

namespace Model4You.Web.Controllers
{
    public class InboxController : Controller
    {
        private readonly IBookingService bookService;
        public const int PerPage = 10;

        public InboxController(IBookingService bookService)
        {
            this.bookService = bookService;
        }

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
    }
}