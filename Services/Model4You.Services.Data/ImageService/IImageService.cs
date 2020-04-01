using System.Threading.Tasks;

namespace Model4You.Services.Data.ImageService
{
    public interface IImageService
    {
        Task<string> DeleteImage(int imgId);

        Task<string> DeleteProfilePicture(string userId);

        Task<string> ChangeProfilePicture(string imageUrl, string userId, int imageId);
    }
}