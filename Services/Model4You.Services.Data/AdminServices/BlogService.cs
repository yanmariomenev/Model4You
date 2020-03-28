using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Services.Data.AdminServices
{
    public class BlogService : IBlogService
    {
        private readonly IDeletableEntityRepository<Blog> blogRepository;
        private readonly IDeletableEntityRepository<BlogContent> blogContentRepository;

        public BlogService(
            IDeletableEntityRepository<Blog> blogRepository,
            IDeletableEntityRepository<BlogContent> blogContentRepository)
        {
            this.blogRepository = blogRepository;
            this.blogContentRepository = blogContentRepository;
        }

        public async Task CreateBlog(string title, string imageUrl, string userId)
        {
          var blog = new Blog
          {
              UserId = userId,
              Title = title,
              ImageUrl = imageUrl,
          };

          await this.blogRepository.AddAsync(blog);
          await this.blogRepository.SaveChangesAsync();
        }

        public async Task CreateBlogContent(string title, string content)
        {
            var getBlogId = await blogRepository.All()
                .Where(x => x.Title == title).Select(x => x.Id)
                .FirstOrDefaultAsync();

            var blogContent = new BlogContent
            {
                BlogId = getBlogId,
                Title = title,
                Content = content,
            };

            await this.blogContentRepository.AddAsync(blogContent);
            await this.blogContentRepository.SaveChangesAsync();
        }
    }
}