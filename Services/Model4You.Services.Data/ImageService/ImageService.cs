﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Services.Data.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<UserImage> imagesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public ImageService(IDeletableEntityRepository<UserImage> imagesRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.imagesRepository = imagesRepository;
            this.userRepository = userRepository;
        }

        public async Task<string> DeleteImage(int imgId)
        {
            var imageToRemove = await this.imagesRepository
                .All()
                .Where(x => x.Id == imgId)
                .FirstOrDefaultAsync();

            this.imagesRepository.Delete(imageToRemove);
            await this.imagesRepository.SaveChangesAsync();
            return "Deleted";
        }

        public async Task<string> DeleteProfilePicture(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return "Invalid user";
            }

            var imageToRemove = await this.userRepository
                .All()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();
            imageToRemove.ProfilePicture =
                "https://res.cloudinary.com/dpp9tqhjn/image/upload/v1585748850/images/no-avatar-png-8_rbh1ni.png";
            await this.userRepository.SaveChangesAsync();

            return "Deleted";
        }

        public async Task<string> ChangeProfilePicture(string imageUrl, string userId, int imageId)
        {
            if (imageUrl == null && userId == null)
            {
                return "Failed! invalid user or image";
            }

            var profilePicture = await this.userRepository
                .All()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            profilePicture.ProfilePicture = imageUrl;

            await this.DeleteImage(imageId);

            return "Changed profile picture";
        }
    }
}