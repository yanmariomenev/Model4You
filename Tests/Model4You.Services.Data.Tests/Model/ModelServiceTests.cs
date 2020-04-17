﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Models.Enums;
using Model4You.Data.Repositories;
using Model4You.Services.Data.ModelService;
using Model4You.Services.Mapping;
using Model4You.Web.ViewModels.Model;

namespace Model4You.Services.Data.Tests.Model
{
    using Xunit;
    using Moq;

    public class ModelServiceTests : BaseServiceTest
    {

        [Fact]
        public async Task GetModelCount_ShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);


            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var user3 = await this.CreateUserAsync("Ri@abv.bg", "Ri", "Ro", repository);
            var count = await service.GetModelCount();

            Assert.Equal(3, count);

        }

        [Fact]
        public async Task GetModelCount_ShouldReturnZero_IfUserHasNoPictureOrInformation()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserWithNoInformationAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserWithNoInformationAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var user3 = await this.CreateUserWithNoInformationAsync("Ri@abv.bg", "Ri", "Ro", repository);
            var count = await service.GetModelCount();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetModelById_ShouldReturnModelWithTheSameId()
        {
            //AutoMapperConfig.RegisterMappings(typeof(ProfileViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var user3 = await this.CreateUserAsync("Ri@abv.bg", "Ri", "Ro", repository);
            var getModel = await service.GetModelById<ProfileViewModel>(user1);
            var getModelTest2 = await service.GetModelById<ProfileViewModel>(user2);
            Assert.Equal(user1, getModel.Id);
            Assert.Equal(user2, getModelTest2.Id);

        }

        [Fact]
        public async Task GetModelById_WithWrongDataShouldReturnNothing()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var user3 = await this.CreateUserAsync("Ri@abv.bg", "Ri", "Ro", repository);

            var getModel = await service.GetModelById<ProfileViewModel>("TESTID123");
            Assert.Null(getModel);
        }

        [Fact]
        public async Task InsertModelInformation_ShouldAddFillTablesWithDefaultInformation()
        {
            //AutoMapperConfig.RegisterMappings(typeof(ProfileViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var modelInfoRepository = new EfDeletableEntityRepository<ModelInformation>(new ApplicationDbContext(options));
            var service = new ModelService.ModelService(userRepository, null, null, modelInfoRepository);

            var user1 = await this.CreateUserWithNoInformationAsync("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            var user2 = await this.CreateUserWithNoInformationAsync("Vank@abv.bg", "Vank", "Vanko", userRepository);
            await service.InsertModelInformation(user1);

            var check = await modelInfoRepository.All().Where(x => x.UserId == user1).FirstOrDefaultAsync();

            Assert.NotNull(check);

        }

        [Fact]
        public async Task InsertModelInformation_CallingTheMethodTwiceShouldReturnRightStatusMassages()
        {
            string InfoUpdate = "User Information Updated";
            string InfoCreated = "User Information was created";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));
            var modelInfoRepository = new EfDeletableEntityRepository<ModelInformation>(new ApplicationDbContext(options));
            var service = new ModelService.ModelService(userRepository, null, null, modelInfoRepository);

            var user1 = await this.CreateUserWithNoInformationAsync("pesho@abv.bg", "Pesho", "Peshev", userRepository);
            var user2 = await this.CreateUserWithNoInformationAsync("Vank@abv.bg", "Vank", "Vanko", userRepository);
            var response = await service.InsertModelInformation(user1);
            var response2 = await service.InsertModelInformation(user1);
            Assert.Equal(InfoCreated, response);
            Assert.Equal(InfoUpdate, response2);

        }

        [Fact]
        public async Task ChangeUserFirstName_ShouldChangeUsersFirstNameWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var nameChangeExample = "Sancho";
            var status = "Success";

            var changeUserFirstName = await service.ChangeUserFirstName(getUser, nameChangeExample);
            var userCurrentNameSecondCheck = getUser.FirstName;

            Assert.Equal(status, changeUserFirstName);
            Assert.Equal(nameChangeExample, userCurrentNameSecondCheck);
        }

        [Fact]
        public async Task ChangeUserFirstNameWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var nameChangeExample = "Sancho";
            var statusInvalid = "Invalid user";
            
            var changeUserFirstName = await service.ChangeUserFirstName(null, nameChangeExample);
            var userCurrentNameSecondCheck = getUser.FirstName;

            Assert.Equal(statusInvalid, changeUserFirstName);
            Assert.Equal("Pesho", userCurrentNameSecondCheck);
        }

        [Fact]
        public async Task ChangeUserLastName_ShouldChangeUsersFirstNameWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var nameChangeExample = "Sanchev";
            var status = "Success";

            var changeUserLastName = await service.ChangeUserLastName(getUser, nameChangeExample);
            var userCurrentNameSecondCheck = getUser.LastName;

            Assert.Equal(status, changeUserLastName);
            Assert.Equal(nameChangeExample, userCurrentNameSecondCheck);
        }

        [Fact]
        public async Task ChangeUserLastNameWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var nameChangeExample = "Sanchev";
            var statusInvalid = "Invalid user";

            var changeUserLastName = await service.ChangeUserFirstName(null, nameChangeExample);
            var userCurrentNameSecondCheck = getUser.LastName;

            Assert.Equal(statusInvalid, changeUserLastName);
            Assert.Equal("Peshev", userCurrentNameSecondCheck);
        }

        [Fact]
        public async Task ChangeUserAge_ShouldChangeUserAgeWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var ageExample = 68;
            var status = "Success";

            var changeUserAge = await service.ChangeUserAge(getUser, ageExample);
            var userCurrentAgeSecondCheck = getUser.ModelInformation.Age;

            Assert.Equal(status, changeUserAge);
            Assert.Equal(ageExample, userCurrentAgeSecondCheck);
        }

        [Fact]
        public async Task ChangeUserAgeWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var ageExample = 68;
            var statusInvalid = "Invalid user";

            var changeUserAge = await service.ChangeUserAge(null, ageExample);
            var userCurrentAgeSecondCheck = getUser.ModelInformation.Age;

            Assert.Equal(statusInvalid, changeUserAge);
            Assert.Equal(13, userCurrentAgeSecondCheck);
        }

        [Fact]
        public async Task ChangeUserGender_ShouldChangeUsersGenderWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var changeTo = Gender.Female;
            var status = "Success";

            var changeUserGender = await service.ChangeUserGender(getUser, changeTo);
            var userCurrentGenderSecondCheck = getUser.ModelInformation.Gender;

            Assert.Equal(status, changeUserGender);
            Assert.Equal(changeTo, userCurrentGenderSecondCheck);
        }

        [Fact]
        public async Task ChangeUserGenderWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var changeTo = Gender.Female;
            var statusInvalid = "Invalid user";

            var changeUserGender = await service.ChangeUserGender(null, changeTo);
            var userCurrentGenderSecondCheck = getUser.ModelInformation.Gender;

            Assert.Equal(statusInvalid, changeUserGender);
            Assert.Equal(Gender.Male, userCurrentGenderSecondCheck);
        }

        [Fact]
        public async Task ChangeUserEthnicity_ShouldChangeUsersEthnicityWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var changeTo = Ethnicity.Japanese;
            var status = "Success";

            var changeUserEthnicity = await service.ChangeUserEthnicity(getUser, changeTo);
            var userCurrentEthnicitySecondCheck = getUser.ModelInformation.Ethnicity;

            Assert.Equal(status, changeUserEthnicity);
            Assert.Equal(changeTo, userCurrentEthnicitySecondCheck);
        }

        [Fact]
        public async Task ChangeUserEthnicityWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var changeTo = Ethnicity.Japanese;
            var statusInvalid = "Invalid user";

            var changeUserEthnicity = await service.ChangeUserEthnicity(null, changeTo);
            var userCurrentEthnicitySecondCheck = getUser.ModelInformation.Ethnicity;

            Assert.Equal(statusInvalid, changeUserEthnicity);
            Assert.Equal(Ethnicity.Chinese, userCurrentEthnicitySecondCheck);
        }

        [Fact]
        public async Task ChangeUserValuesHips_ShouldChangeUserValuesWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var hipsInput = "hips";
            var hipsExample = 20;
            var status = "Success";

            var changeUserValues = await service.ChangeUserValues(getUser, hipsExample, hipsInput);
            var userCurrentValueSecondCheck = getUser.ModelInformation.Hips;

            Assert.Equal(status, changeUserValues);
            Assert.Equal(hipsExample, userCurrentValueSecondCheck);
        }

        [Fact]
        public async Task ChangeUserValuesBust_ShouldChangeUserValuesWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var bustInput = "bust";
            var bustExample = 20;
            var status = "Success";

            var changeUserValues = await service.ChangeUserValues(getUser, bustExample, bustInput);
            var userCurrentValueSecondCheck = getUser.ModelInformation.Bust;

            Assert.Equal(status, changeUserValues);
            Assert.Equal(bustExample, userCurrentValueSecondCheck);
        }

        [Fact]
        public async Task ChangeUserValuesWaist_ShouldChangeUserValuesWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var valueInput = "waist";
            var valueExample = 20;
            var status = "Success";

            var changeUserValues = await service.ChangeUserValues(getUser, valueExample, valueInput);
            var userCurrentValueSecondCheck = getUser.ModelInformation.Waist;

            Assert.Equal(status, changeUserValues);
            Assert.Equal(valueExample, userCurrentValueSecondCheck);
        }

        [Fact]
        public async Task ChangeUserValuesHeight_ShouldChangeUserValuesWithStatusSuccess()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var valueInput = "height";
            var valueExample = 20;
            var status = "Success";

            var changeUserValues = await service.ChangeUserValues(getUser, valueExample, valueInput);
            var userCurrentValueSecondCheck = getUser.ModelInformation.Height;

            Assert.Equal(status, changeUserValues);
            Assert.Equal(valueExample, userCurrentValueSecondCheck);
        }

        [Fact]
        public async Task ChangeUserValuesWithNullUser_ShouldReturnStatusInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var service = new ModelService.ModelService(repository, null, null, null);

            var user1 = await this.CreateUserAsync("pesho@abv.bg", "Pesho", "Peshev", repository);
            var user2 = await this.CreateUserAsync("Vank@abv.bg", "Vank", "Vanko", repository);
            var getUser = await repository.All().Where(x => x.Id == user1).FirstOrDefaultAsync();
            var valueExample = 20;
            var statusInvalid = "Invalid user";

            var changeUserAge = await service.ChangeUserAge(null, valueExample);
            var userCurrentHipsSizeSecondCheck = getUser.ModelInformation.Hips;
            var userCurrentWaistSizeSecondCheck = getUser.ModelInformation.Waist;

            Assert.Equal(statusInvalid, changeUserAge);

            // Checked only with two properties no need for more.
            Assert.Equal(14, userCurrentHipsSizeSecondCheck);
            Assert.Equal(16, userCurrentWaistSizeSecondCheck);
        }


        private async Task<string> CreateUserAsync(string email, string name, string lastName, IDeletableEntityRepository<ApplicationUser> repo)
        {
            var user = new ApplicationUser()
            {
                FirstName = name,
                LastName = lastName,
                Email = email,
                UserName = email,
                ProfilePicture = "Test.com",
                ModelInformation = new ModelInformation
                {
                    Age = 13,
                    Bust = 14,
                    Waist = 16,
                    Hips = 14,
                    Height = 15,
                    Gender = Gender.Male,
                    Ethnicity = Ethnicity.Chinese,
                },
            };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();
            return user.Id;
        }

        // Create user with no profile picture and modelInformation
        private async Task<string> CreateUserWithNoInformationAsync(string email, string name, string lastName, IDeletableEntityRepository<ApplicationUser> repo)
        {
            var user = new ApplicationUser()
            {
                FirstName = name,
                LastName = lastName,
                Email = email,
                UserName = email,
            };
            await repo.AddAsync(user);
            await repo.SaveChangesAsync();
            return user.Id;
        }
    }
}