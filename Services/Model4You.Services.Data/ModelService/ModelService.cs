using System;
using System.Runtime.CompilerServices;
using Model4You.Data.Models.Enums;

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

        public ModelService(IDeletableEntityRepository<ApplicationUser> appRepository)
        {
            this.appRepository = appRepository;
        }

        public async Task<IEnumerable<T>> TakeSixModels<T>()
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null).Take(6);
            return await user.To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> TakeAllModels<T>()
        {
            // If you have modelInformation then take the model and display her/him.
            // Admins and Moderators will be not displayed.
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null);
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