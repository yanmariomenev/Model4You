using Model4You.Services.Data.ModelService;
using Model4You.Web.ViewModels.ModelViews;
using Model4You.Web.ViewModels.Settings;

namespace Model4You.Web.Controllers
{
    using System.Diagnostics;

    using Model4You.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IModelService modelService;

        public HomeController(IModelService modelService)
        {
            this.modelService = modelService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                    this.modelService.TakeSixModels<ModelProfileView>(),
            };
            var v = this.modelService.TakeSixModels<ModelProfileView>();
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
