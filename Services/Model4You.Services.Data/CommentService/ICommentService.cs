namespace Model4You.Services.Data.CommentService
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task Create(int id, string name, string email, string content);
    }
}