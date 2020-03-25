using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Model4You.Services.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName);
    }
}