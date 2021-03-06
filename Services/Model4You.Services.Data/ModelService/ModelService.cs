﻿namespace Model4You.Services.Data.ModelService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Data.Models.Enums;
    using Model4You.Services.Cloudinary;
    using Model4You.Services.Mapping;

    public class ModelService : IModelService
    {
        private const string InfoUpdate = "User Information Updated";
        private const string InfoCreated = "User Information was created";
        private const string SuccessfullyUpdated = "Success";
        private const string InvalidUser = "Invalid user";
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

        public async Task<int> GetPagesCount(int perPage)
        {
            var profiles =
               await this.appRepository
                .All()
                .Where(x => x.ModelInformation != null && !x.IsDeleted && x.ProfilePicture != null)
                .CountAsync();
            var count = (int)Math.Ceiling(profiles / (decimal)perPage);

            return count;
        }

        public async Task<int> GetModelCount()
        {
            var modelCount = await this.appRepository
                .All()
                .Where(x =>
                    x.ModelInformation != null &&
                    !x.IsDeleted &&
                    x.ProfilePicture != null)
                .CountAsync();

            return modelCount;
        }

        public async Task<IEnumerable<T>> TakeAllPictures<T>(string userId)
        {
            var pictures = this.appRepository.All()
                .Where(x => x.Id == userId && x.UserImages.Any());

            return await pictures.To<T>().ToListAsync();
        }

        public async Task<int> UploadAlbum(List<string> imageUrl, string userId)
        {
            // TODO Make this better.
            foreach (var images in imageUrl
                .Select(image => new UserImage { UserId = userId, ImageUrl = image }))
            {
                await this.imageRepository.AddAsync(images);
                await this.imageRepository.SaveChangesAsync();
            }

            return imageUrl.Count;
        }

        public async Task<IEnumerable<T>> TakeSixModels<T>()
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null && !x.IsDeleted && x.ProfilePicture != null)
                .OrderBy(x => Guid.NewGuid())
                .Take(6);
            return await user.To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> TakeAllModels<T>(int page, int perPage)
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null && !x.IsDeleted && x.ProfilePicture != null)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(perPage * (page - 1))
                .Take(perPage);
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

        public async Task<string> InsertModelInformation(string id)
        {
            var checkUser = await this.modelInformationRepository
                .All()
                .Where(x => x.UserId == id).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                return InfoUpdate;
            }

            var modelInformation = new ModelInformation
            {
                UserId = id,
                Country = "None",
                City = "None",
                Age = 16,
                Bust = 0,
                Hips = 0,
                Height = 0,
                Ethnicity = Enum.Parse<Ethnicity>("Other"),
                Gender = Enum.Parse<Gender>("Other"),
                Nationality = "Bulgarian",
            };
            await this.modelInformationRepository.AddAsync(modelInformation);
            await this.modelInformationRepository.SaveChangesAsync();
            return InfoCreated;
        }

        public async Task<string> ChangeUserFirstName(ApplicationUser user, string firstName)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            user.FirstName = firstName;
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserLastName(ApplicationUser user, string lastName)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            user.LastName = lastName;
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserAge(ApplicationUser user, int age)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            user.ModelInformation.Age = age;
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserGender(ApplicationUser user, Gender gender)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            user.ModelInformation.Gender = gender;
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserEthnicity(ApplicationUser user, Ethnicity ethnicity)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            user.ModelInformation.Ethnicity = ethnicity;
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserValues(ApplicationUser user, double value, string property)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            // Implement Default case.
            _ = property switch
            {
                "hips" => user.ModelInformation.Hips = value,
                "height" => user.ModelInformation.Height = value,
                "waist" => user.ModelInformation.Waist = value,
                "bust" => user.ModelInformation.Bust = value,
            };
            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }

        public async Task<string> ChangeUserStringValues(ApplicationUser user, string value, string property)
        {
            if (user == null)
            {
                return InvalidUser;
            }

            // Using discard for this but maybe will change it back to something that makes sense(var result) :).
            _ = property switch
            {
                "modelType" => user.ModelInformation.ModelType = value,
                "nationality" => user.ModelInformation.Nationality = value,
                "instagramUrl" => user.ModelInformation.InstagramUrl = value,
                "facebookUrl" => user.ModelInformation.FacebookUrl = value,
                "city" => user.ModelInformation.City = value,
                "country" => user.ModelInformation.Country = value,
                _ => "Error",
            };

            await this.appRepository.SaveChangesAsync();
            return SuccessfullyUpdated;
        }
    }
}
