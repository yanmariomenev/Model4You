using System.Threading.Tasks;
using Model4You.Services.Data.ContactFormService;
using Model4You.Web.ViewModels.Home.ContactView;

namespace Model4You.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.ModelService;
    using Model4You.Web.ViewModels;
    using Model4You.Web.ViewModels.ModelViews;
    using Model4You.Web.ViewModels.Settings;

    public class HomeController : BaseController
    {
        private readonly IModelService modelService;
        private readonly IContactFormService contactService;
        public HomeController(IModelService modelService, IContactFormService contactService)
        {
            this.modelService = modelService;
            this.contactService = contactService;
        }

        public IActionResult Index()
        {
            // TODO Make getting models async
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                    this.modelService.TakeSixModels<ModelProfileView>(),
            };
            return this.View(viewModel);
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
           await this.contactService.Create(model.Name, model.Email, model.Subject, model.Message); 
           return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
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