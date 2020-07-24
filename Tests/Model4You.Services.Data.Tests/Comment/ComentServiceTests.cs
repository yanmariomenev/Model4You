namespace Model4You.Services.Data.Tests.Comment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Data.Repositories;
    using Xunit;

    public class ComentServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task Create_ShouldReturnCreatedCommentForTheBlogThatIsAssignedTo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<Model4You.Data.Models.Blog>(new ApplicationDbContext(options));
            var commentRepository = new EfDeletableEntityRepository<BlogComment>(new ApplicationDbContext(options));
            var blogContentRepository = new EfDeletableEntityRepository<BlogContent>(new ApplicationDbContext(options));

            var service = new CommentService.CommentService(commentRepository);

            var createdBlogId = await this.CreateBlogForTest
                ("testTitle", "TestUrl", "TestUserId", repository);

            var createBlogContentId =
                await this.CreateBlogContentForTests(
                    "Test", "TestContent", createdBlogId, blogContentRepository);

            await service.Create(createBlogContentId, "Sancho", "Sancho@abv.bg", "Test");
            var takeComment = await commentRepository
                .All()
                .Where(x => x.BlogContentId == createBlogContentId)
                .FirstOrDefaultAsync();

            Assert.Equal(createBlogContentId, takeComment.BlogContentId);
            Assert.NotNull(takeComment);
        }

        private async Task<int> CreateBlogContentForTests(string title, string content, int blogId, IDeletableEntityRepository<BlogContent> cRepo)
        {
            var blogContent = new BlogContent
            {
                BlogId = blogId,
                Title = title,
                Content = content,
            };

            await cRepo.AddAsync(blogContent);
            await cRepo.SaveChangesAsync();
            return blogContent.Id;
        }

        private async Task<int> CreateCommentForTests(
            int id,
            string name,
            string email,
            string content,
            IDeletableEntityRepository<BlogComment> commentRepo)
        {
            var comment = new BlogComment
            {
                BlogContentId = id,
                Name = name,
                Email = email,
                Content = content,
            };

            await commentRepo.AddAsync(comment);
            await commentRepo.SaveChangesAsync();
            return comment.Id;
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
    }
}
