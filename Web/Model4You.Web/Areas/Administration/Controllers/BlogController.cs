using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Services.Data.AdminServices;
using Model4You.Web.ViewModels.Administration.Blog;

namespace Model4You.Web.Areas.Administration.Controllers
{
    public class BlogController : AdministrationController
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.blogService.CreateBlog(input.Title, input.ImageUrl, userId);
            await this.blogService.CreateBlogContent(input.Title, input.Content);

            return this.RedirectToAction(
                "Blog",
                "Blog",
                new { area = string.Empty });
        }
    }
}