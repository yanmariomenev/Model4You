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
        public async Task InsertModelInformation_CallingTheMethodTwiceShoudReturnRightStatusMassages()
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
        public async Task ChangeUserFirstNameWithNullUser_ShoudReturnStatusInvalidUser()
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
            var statusInvalid = "Invalid user";
            
            var changeUserFirstName = await service.ChangeUserFirstName(null, nameChangeExample);
            var userCurrentNameSecondCheck = getUser.FirstName;

            Assert.Equal(statusInvalid, changeUserFirstName);
            Assert.Equal("Pesho", userCurrentNameSecondCheck);
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