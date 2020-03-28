using CloudinaryDotNet;
using Model4You.Services.Cloudinary;

namespace Model4You.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Model4You.Data;
    using Model4You.Data.Common;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Data.Repositories;
    using Model4You.Data.Seeding;
    using Model4You.Services.Data;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Services.Data.ContactFormService;
    using Model4You.Services.Data.ModelService;
    using Model4You.Services.Mapping;
    using Model4You.Services.Messaging;
    using Model4You.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });
            services.AddControllersWithViews(configure =>
                {
                    configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });

            var cloudinaryCredentials = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            var cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.AddSingleton(cloudinaryUtility);

            services.AddRazorPages();
            services.AddSingleton(this.configuration);
            // Data transfer Compression
            services.AddResponseCompression(options => { options.EnableForHttps = true; });

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IContactFormService, ContactFormService>();
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<IContactDataService, ContactDataService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IBlogService, BlogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseStatusCodePagesWithRedirects("/Home/Error");
                //app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Home/Error");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Data transfer Compression
            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        //endpoints.MapControllerRoute("Administration", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
