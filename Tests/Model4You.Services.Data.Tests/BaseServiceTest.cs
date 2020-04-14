using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Repositories;
using Model4You.Services.Cloudinary;
using Model4You.Services.Data.AdminServices;
using Model4You.Services.Data.BookingService;
using Model4You.Services.Data.CommentService;
using Model4You.Services.Data.ContactFormService;
using Model4You.Services.Data.ImageService;
using Model4You.Services.Data.ModelService;
using Model4You.Services.Data.SearchService;
using Model4You.Services.Mapping;
using Model4You.Services.Messaging;
using Model4You.Web.ViewModels.ModelViews;

namespace Model4You.Services.Data.Tests
{
    public abstract class BaseServiceTest : IDisposable
    {
        protected BaseServiceTest()
        {
            var service = this.SetServices();

            this.ServiceProvider = service.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Application services
            services.AddTransient<IEmailSender>(serviceProvider => new SendGridEmailSender("SendGrid:ApiKey"));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IContactFormService, ContactFormService.ContactFormService>();
            services.AddTransient<IModelService, ModelService.ModelService>();
            services.AddTransient<IContactDataService, ContactDataService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IImageService, ImageService.ImageService>();
            services.AddTransient<ICommentService, CommentService.CommentService>();
            services.AddTransient<ISearchService, SearchService.SearchService>();
            services.AddTransient<IBookingService, BookingService.BookingService>();

            AutoMapperConfig.RegisterMappings(typeof(ModelProfileView).GetTypeInfo().Assembly);
            return services;
        }
    }
}