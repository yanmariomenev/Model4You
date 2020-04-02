using System.Threading.Tasks;

namespace Model4You.Services.Data.CommentService
{
    public interface ICommentService
    {
        Task Create(int id, string name, string email, string content);
    }
}