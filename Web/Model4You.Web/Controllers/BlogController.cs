namespace Model4You.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Services.Data.CommentService;
    using Model4You.Web.ViewModels.BindingModels;
    using Model4You.Web.ViewModels.Blog;

    public class BlogController : Controller
    {
        public const int TakeBlogs = 3;
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

               // Display 3 random blogs to the user.
               SideBlogViewModels = await this.blogService.TakeRandomBlogs<BlogViewModel>(TakeBlogs),
               CurrentPage = page,
               PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> BlogArticle(int id)
        {
            var content = await this.blogService.GetBlogContent<BlogContentView>(id);
            if (content == null)
            {
                return this.NotFound();
            }

            var viewModel = new BlogArticleBindingViewModel
            {
                BlogContentView = content,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(BlogArticleBindingViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(
                    "BlogArticle",
                    "Blog",
                    new { id = input.CommentInputModel.BlogId });
            }

            // If the state is valid create a comment for the current blog.
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