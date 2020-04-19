using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Repositories;
using Model4You.Services.Data.AdminServices;
using Model4You.Web.ViewModels.Blog;
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

        [Fact]
        public async Task TakeThreeBlogs_ShouldReturnThreeBlogs()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            for (int i = 0; i < 5; i++)
            {
                await this.CreateBlogForTest
                    ($"testTitle{i}", $"TestUrl{i}", $"TestUserId{i}", repository);
            }

            var takeBlogs = await service.TakeThreeBlogs<BlogViewModel>();
            var count = takeBlogs.Count();

            Assert.Equal(3, count);
        }

        [Fact]
        public async Task TakeThreeBlogs_WithLessThanThreeBlogs_ShouldReturnLessThanThree()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            for (int i = 0; i < 2; i++)
            {
                await this.CreateBlogForTest
                    ($"testTitle{i}", $"TestUrl{i}", $"TestUserId{i}", repository);
            }

            var takeBlogs = await service.TakeThreeBlogs<BlogViewModel>();
            var count = takeBlogs.Count();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task TakeAllBlogs_DependingOnPerPage_ShouldReturnAllBlogsDependingOnPerPageNumber()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            for (int i = 0; i < 8; i++)
            {
                await this.CreateBlogForTest
                    ($"testTitle{i}", $"TestUrl{i}", $"TestUserId{i}", repository);
            }

            var perPage = 6;
            var pagesCount = await service.GetPagesCount(perPage);
            var takeAllBlogs = await service.TakeAllBlogs<BlogViewModel>(1, perPage);
            var takeAllBlogsPage2 = await service.TakeAllBlogs<BlogViewModel>(2, perPage);
            var blogsReturned = takeAllBlogs.Count();
            var blogsReturnedPage2 = takeAllBlogsPage2.Count();
            
            // First page should return 6 and second 2 for overall 8 blogs
            Assert.Equal(6, blogsReturned);
            Assert.Equal(2, blogsReturnedPage2);
            Assert.Equal(2, pagesCount);

        }

        [Fact]
        public async Task TakeRandomBlogs_ShouldReturnRandomMixBlogsDependingOnTheCountThatIsGiven()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);

            for (int i = 0; i < 9; i++)
            {
                await this.CreateBlogForTest
                    ($"testTitle{i}", $"TestUrl{i}", $"TestUserId{i}", repository);
            }

            var count = 7;
            var getRandomBlogsFirst = await service.TakeRandomBlogs<BlogViewModel>(count);
            var getRandomBlogsSecond = await service.TakeRandomBlogs<BlogViewModel>(count);
            // RandomShould return randomly picked blogs and it's always randomly sorted;
            // Guid is used for TakeRandomBlogs;

            Assert.NotEqual(getRandomBlogsFirst, getRandomBlogsSecond);
        }

        [Fact]
        public async Task TakeBlogContent_ShouldReturnTheBlogContent_FromTheGivenId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new BlogService(repository, blogContentRepository);
            var firstBlogId = await this.CreateBlogForTest
                ($"testTitle", $"TestUrl", $"TestUserId", repository);
            for (int i = 0; i < 3; i++)
            {
                await this.CreateBlogForTest
                    ($"testTitle{i}", $"TestUrl{i}", $"TestUserId{i}", repository);
            }

            await this.CreateBlogContentForTests(
                "testTitle",
                "insert random content",
                firstBlogId,
                blogContentRepository);
            var getBlogContent = service.GetBlogContent<BlogContentView>(firstBlogId);
            // Check if it returns the correct title and content;
            Assert.Equal("testTitle", getBlogContent.Result.Title);
            Assert.Equal("insert random content", getBlogContent.Result.Content);
        }

        private async Task<int> CreateBlogForTest(
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
            return blog.Id;
        }

        private async Task CreateBlogContentForTests(string title, string content, int blogId, IDeletableEntityRepository<BlogContent> cRepo)
        {
            var blogContent = new BlogContent
            {
                BlogId = blogId,
                Title = title,
                Content = content,
            };

            await cRepo.AddAsync(blogContent);
            await cRepo.SaveChangesAsync();
        }
    }
}