using Model4You.Services.Data.CommentService;
using Model4You.Web.ViewModels.BindingModels;

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
        private readonly ICommentService commentService;

        public BlogController(
            IBlogService blogService,
            ICommentService commentService)
        {
            this.blogService = blogService;
            this.commentService = commentService;
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
            var v = new BlogArticleBindingViewModel
            {
                BlogContentView = viewModel,
            };
            return this.View(v);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(BlogArticleBindingViewModel input)
        {
           await this.commentService.Create(
               input.CommentInputModel.BlogContentId,
               input.CommentInputModel.Name,
               input.CommentInputModel.Email,
               input.CommentInputModel.Content);
           return this.RedirectToAction(
               "BlogArticle",
               "Blog",
               new { id = input.CommentInputModel.BlogId });
        }
    }
}