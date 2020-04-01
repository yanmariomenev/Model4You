namespace Model4You.Services.Data.ImageService
{
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> DeleteImage(int imgId);

        Task<string> DeleteProfilePicture(string userId);

        Task<string> ChangeProfilePicture(string imageUrl, string userId, int imageId);
    }
}