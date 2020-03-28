using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Services.Data.AdminServices;
using Model4You.Web.ViewModels.Blog;

namespace Model4You.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public async Task<IActionResult> Blog()
        {
            var viewModel = new BlogIndexViewModel
            {
               BlogViewModels = await this.blogService.TakeAllBlogs<BlogViewModel>(),
            };

            return View(viewModel);
        }
    }
}