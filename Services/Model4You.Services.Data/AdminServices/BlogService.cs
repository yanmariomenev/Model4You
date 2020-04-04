using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Services.Mapping;

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

        public async Task<int> GetPagesCount(int perPage)
        {
            var blogs =
                await this.blogRepository
                    .All()
                    .Where(x => !x.IsDeleted)
                    .CountAsync();
            var count = (int)Math.Ceiling(blogs / (decimal)perPage);

            return count;
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

        public async Task<IEnumerable<T>> TakeAllBlogs<T>(int page, int perPage)
        {
            var blogs = this.blogRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Skip(perPage * (page - 1))
                .Take(perPage);

            return await blogs.To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> TakeThreeBlogs<T>()
        {
            var blogs = this.blogRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Take(3);
            return await blogs.To<T>().ToListAsync();
        }

        public async Task<T> GetBlogContent<T>(int id)
        {
            var content = await this.blogContentRepository
                .All()
                .Where(x => x.BlogId == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return content;
        }
    }
}