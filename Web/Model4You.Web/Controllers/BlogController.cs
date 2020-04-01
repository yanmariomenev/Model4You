namespace Model4You.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Web.ViewModels.Blog;

    public class BlogController : Controller
    {
        public const int BlogPerPage = 6;
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public async Task<IActionResult> Blog(int page = 1, int perPage = BlogPerPage)
        {
            var pagesCount = await this.blogService.GetPagesCount(perPage);

            var viewModel = new BlogIndexViewModel
            {
               BlogViewModels = await this.blogService.TakeAllBlogs<BlogViewModel>(page, perPage),
               CurrentPage = page,
               PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> BlogArticle(int id)
        {
            var viewModel = await this.blogService.GetBlogContent<BlogContentView>(id);
            return this.View(viewModel);
        }
    }
}