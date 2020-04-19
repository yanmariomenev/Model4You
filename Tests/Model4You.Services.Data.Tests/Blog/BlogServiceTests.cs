using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Repositories;
using Model4You.Services.Data.AdminServices;
using Xunit;

namespace Model4You.Services.Data.Tests.Blog
{
    public class BlogServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task Create_ShouldCreateBlog()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            await service.CreateBlog("testTitle", "TestUrl", "TestUserId");
            await service.CreateBlog("testTitle2", "TestUrl2", "TestUserId2");
            var blogId = await repository
                .All()
                .Where(x => x.Title == "testTitle")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            var blogId2 = await repository
                .All()
                .Where(x => x.Title == "testTitle2")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            await service.CreateBlogContent("testTitle", "This is test content");

            // Check if there is content for this blogId;
            var blogWithContent = await blogContentRepository
                .All()
                .Where(x => x.BlogId == blogId)
                .Select(x => x.Content)
                .FirstOrDefaultAsync();
            var blogWithContent2 = await blogContentRepository
                .All()
                .Where(x => x.BlogId == blogId2)
                .Select(x => x.Content)
                .FirstOrDefaultAsync();

            Assert.NotNull(blogWithContent);
            Assert.Null(blogWithContent2);
        }

        [Fact]
        public async Task CreateBlogContent_ShouldCreateBlogContentForThatTitle()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            await service.CreateBlog("testTitle", "TestUrl", "TestUserId");
            await service.CreateBlog("testTitle2", "TestUrl2", "TestUserId2");
            var count = await repository.All().CountAsync();
            var title = await repository.All().Where(x => x.Title == "testTitle").FirstOrDefaultAsync();

            Assert.Equal(2, count);
            Assert.NotNull(title);
        }



        private async Task CreateBlogForTest(
            string title,
            string imageUrl,
            string userId,
            IDeletableEntityRepository<Model4You.Data.Models.Blog> blogRepo)
        {
            var blog = new Model4You.Data.Models.Blog
            {
                UserId = userId,
                Title = title,
                ImageUrl = imageUrl,
            };

            await blogRepo.AddAsync(blog);
            await blogRepo.SaveChangesAsync();
        }
    }
}