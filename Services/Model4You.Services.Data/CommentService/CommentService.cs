namespace Model4You.Services.Data.CommentService
{
    using System.Threading.Tasks;

    using Model4You.Data.Common.Models;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<BlogComment> commentRepository;

        public CommentService(IDeletableEntityRepository<BlogComment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task Create(int id, string name, string email, string content)
        {
            var comment = new BlogComment
            {
                BlogContentId = id,
                Name = name,
                Email = email,
                Content = content,
            };

            await this.commentRepository.AddAsync(comment);
            await this.commentRepository.SaveChangesAsync();
        }
    }
}