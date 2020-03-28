using System;
using System.Runtime.CompilerServices;
using Model4You.Data.Common.Models;
using Model4You.Data.Models.Enums;
using Model4You.Services.Cloudinary;

namespace Model4You.Services.Data.ModelService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ModelService : IModelService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> appRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<UserImage> imageRepository;
        private readonly IDeletableEntityRepository<ModelInformation> modelInformationRepository;

        public ModelService(
            IDeletableEntityRepository<ApplicationUser> appRepository,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<UserImage> imageRepository,
            IDeletableEntityRepository<ModelInformation> modelInformationRepository)
        {
            this.appRepository = appRepository;
            this.cloudinaryService = cloudinaryService;
            this.imageRepository = imageRepository;
            this.modelInformationRepository = modelInformationRepository;
        }

        public async Task<IEnumerable<T>> TakeAllPictures<T>(string userId)
        {
            var pictures = appRepository.All()
                .Where(x => x.Id == userId && x.UserImages.Any());

            return await pictures.To<T>().ToListAsync();
        }

        public async Task UploadAlbum(List<string> imageUrl, string userId)
        {
            var images = new UserImage();
            foreach (var image in imageUrl)
            {
                //var album = new UserImage
                //{
                //    UserId = userId,
                //    ImageUrl = image,
                //};
                images.UserId = userId;
                images.ImageUrl = image;
            }

            await this.imageRepository.AddAsync(images);
            await this.imageRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> TakeSixModels<T>()
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null && x.Location != null).Take(6);
            return await user.To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> TakeAllModels<T>()
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null && x.Location != null);
            return await user.To<T>().ToListAsync();
        }

        public async Task<T> GetModelById<T>(string modelId)
        {
            var model = await this.appRepository.All()
                .Where(u => u.Id == modelId)
                .To<T>().
                FirstOrDefaultAsync();

            return model;
        }

        public async Task InsertModelInformation(string id)
        {
            var modelInformation = new ModelInformation
            {
                UserId = id,
                Age = 16,
                Bust = 0,
                Hips = 0,
                Height = 0,
                Ethnicity = Enum.Parse<Ethnicity>("Other"),
                Gender = Enum.Parse<Gender>("Other"),
                Nationality = "Bulgarian",
            };
            await this.modelInformationRepository.AddAsync(modelInformation);
        }

        public async Task<string> ChangeUserFirstName(ApplicationUser user, string firstName)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            user.FirstName = firstName;
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserLastName(ApplicationUser user, string lastName)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            user.LastName = lastName;
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserAge(ApplicationUser user, int age)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            user.ModelInformation.Age = age;
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserGender(ApplicationUser user, Gender gender)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            user.ModelInformation.Gender = gender;
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserEthnicity(ApplicationUser user, Ethnicity ethnicity)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            user.ModelInformation.Ethnicity = ethnicity;
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserValues(ApplicationUser user, double value, string property)
        {
            if (user == null)
            {
                return "Invalid user";
            }
            var result = property switch
            {
                "hips" => user.ModelInformation.Hips = value,
                "height" => user.ModelInformation.Height = value,
                "waist" => user.ModelInformation.Waist = value,
                "bust" => user.ModelInformation.Bust = value,
            };
            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> ChangeUserStringValues(ApplicationUser user, string value, string property)
        {
            if (user == null)
            {
                return "Invalid user";
            }

            var result = property switch
            {
                "modelType" => user.ModelInformation.ModelType = value,
                "nationality" => user.ModelInformation.Nationality = value,
                "instagramUrl" => user.ModelInformation.InstagramUrl = value,
                "facebookUrl" => user.ModelInformation.FacebookUrl = value,
            };

            await this.appRepository.SaveChangesAsync();
            return "Success";
        }

    }
}