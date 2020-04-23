namespace Model4You.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Common;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Services.Data.ContactFormService;
    using Model4You.Services.Data.ModelService;
    using Model4You.Services.Messaging;
    using Model4You.Web.ViewModels;
    using Model4You.Web.ViewModels.Blog;
    using Model4You.Web.ViewModels.Home.AboutView;
    using Model4You.Web.ViewModels.Home.ContactView;
    using Model4You.Web.ViewModels.ModelViews;
    using Model4You.Web.ViewModels.Search;
    using Model4You.Web.ViewModels.Settings;

    public class HomeController : BaseController
    {
        private readonly IModelService modelService;
        private readonly IContactFormService contactService;
        private readonly IBlogService blogService;
        private readonly IEmailSender emailSender;

        public HomeController(
            IModelService modelService,
            IContactFormService contactService,
            IBlogService blogService,
            IEmailSender emailSender)
        {
            this.modelService = modelService;
            this.contactService = contactService;
            this.blogService = blogService;
            this.emailSender = emailSender;
        }

        // TODO DISPLAY RANDOM 6 MODELS OR TOP MODELS
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                   await this.modelService.TakeSixModels<ModelProfileView>(),
                BlogViewModels = await this.blogService.TakeThreeBlogs<BlogViewModel>(),
                Count = await this.modelService.GetModelCount(),
            };
            return this.View(viewModel);
        }

        public async Task<IActionResult> About()
        {
            var viewModel = new AboutViewModel
            {
                Count = await this.modelService.GetModelCount(),
            };
            return this.View(viewModel);
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.contactService.Create(model.Name, model.Email, model.Subject, model.Message);
            await this.emailSender.SendEmailAsync(
                model.Email,
                model.Name,
                GlobalConstants.SystemEmail,
                model.Subject,
                model.Message);
            return this.RedirectToAction(nameof(this.ThankYou));
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}