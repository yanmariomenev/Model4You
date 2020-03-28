using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model4You.Services.Data.AdminServices
{
    public interface IBlogService
    {
        Task CreateBlog(string title, string imageUrl, string userId);

        Task CreateBlogContent(string title, string content);

        Task<IEnumerable<T>> TakeAllBlogs<T>();

        Task<T> GetBlogContent<T>(int id);
    }
}