using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Repositories;
using Xunit;

namespace Model4You.Services.Data.Tests.Image
{
    public class ImageServiceTests : BaseServiceTest
    {
        private const string Success = "Deleted";
        private const string ChangedProfilePictureSuccess = "Changed profile picture";
        private const string DefaultProfilePicture = 
            "https://res.cloudinary.com/dpp9tqhjn/image/upload/v1585748850/images/no-avatar-png-8_rbh1ni.png";

        [Fact]
        public async Task DeleteImage_ShouldDeleteTheImageByImageId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var imageRepository = new EfDeletableEntityRepository<UserImage>(new ApplicationDbContext(options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ImageService.ImageService(imageRepository, userRepository);

            var imageToDeleteById = await this.CreateImageForTest("TestUrl", "TestUserId", imageRepository);
            for (int i = 0; i < 2; i++)
            {
                await this.CreateImageForTest($"TestUrl{i}", "TestUserId", imageRepository);
            }

            var deleteImage = await service.DeleteImage(imageToDeleteById);
            var imageCount = await imageRepository
                .All()
                .Where(x => x.UserId == "TestUserId")
                .CountAsync();

            Assert.Equal(Success, deleteImage);
            Assert.Equal(2, imageCount);
        }

        [Fact]
        public async Task DeleteProfilePicture_ShouldDeleteProfilePictureAndReplaceItWithDefaultAvatar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var imageRepository = new EfDeletableEntityRepository<UserImage>(new ApplicationDbContext(options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ImageService.ImageService(imageRepository, userRepository);
            var user1 = await this.CreateUserForTests
                ("pesho@abv.bg", "Pesho", "Peshev", "testUrl", userRepository);

            var removeProfilePicture = await service.DeleteProfilePicture(user1);
            var profilePictureDefaultUrlCheck = await userRepository
                    .All()
                    .Select(x => x.ProfilePicture)
                    .FirstOrDefaultAsync();

            Assert.Equal(Success, removeProfilePicture);
            Assert.Equal(DefaultProfilePicture, profilePictureDefaultUrlCheck);
        }

        [Fact]
        public async Task ChangeProfilePicture_ShouldChangeProfilePicture_AndRemoveItFromAlbum()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var imageRepository = new EfDeletableEntityRepository<UserImage>(new ApplicationDbContext(options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ImageService.ImageService(imageRepository, userRepository);
            var user1 = await this.CreateUserForTests
                ("pesho@abv.bg", "Pesho", "Peshev", "testUrl", userRepository);
            var imageFromAlbum = await this.CreateImageForTest("TestUrlFromAlbumImage", user1, imageRepository);

            var changeImage = await service.ChangeProfilePicture
                ("TestUrlFromAlbumImage", user1, imageFromAlbum);
            var currentProfilePicture = await userRepository
                .All()
                .Where(x => x.Id == user1)
                .Select(x => x.ProfilePicture)
                .FirstOrDefaultAsync();

            Assert.Equal(ChangedProfilePictureSuccess, changeImage);
            Assert.Equal("TestUrlFromAlbumImage", currentProfilePicture);
        }

        [Fact]
        public async Task GetImageCountOfCurrentUser_ShouldReturnTheNumberOfImages_ForTheUserGivenById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var imageRepository = new EfDeletableEntityRepository<UserImage>(new ApplicationDbContext(options));
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ImageService.ImageService(imageRepository, userRepository);
            var user1 = await this.CreateUserForTests
                ("pesho@abv.bg", "Pesho", "Peshev", "testUrl", userRepository);
            for (int i = 0; i < 3; i++)
            {
                await this.CreateImageForTest($"TestUrlFromAlbumImage{i}", user1, imageRepository);
            }

            var imageCount = await service.GetImageCountOfCurrentUser(user1);

            Assert.Equal(3, imageCount);

        }

        private async Task<int> CreateImageForTest(string url, string userId, IDeletableEntityRepository<UserImage> repo)
        {
            var userImages = new UserImage
            {
                UserId = userId,
                ImageUrl = url,
            };
            await repo.AddAsync(userImages);
            await repo.SaveChangesAsync();
            return userImages.Id;
        }

        private async Task<string> CreateUserForTests
            (string email, string name, string lastName, string url, IDeletableEntityRepository<ApplicationUser> repo)
        {
            var user = new ApplicationUser()
            {
                FirstName = name,
                LastName = lastName,
                Email = email,
                UserName = email,
                ProfilePicture = url,
            };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();
            return user.Id;
        }
    }
}