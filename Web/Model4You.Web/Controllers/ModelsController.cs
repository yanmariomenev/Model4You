using Microsoft.AspNetCore.Mvc;
using Model4You.Services.Data.ModelService;
using Model4You.Web.ViewModels.ModelViews;

namespace Model4You.Web.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IModelService modelService;

        public ModelsController(IModelService modelService)
        {
            this.modelService = modelService;
        }
        // GET
        public IActionResult Model()
        {
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                    this.modelService.TakeAllModels<ModelProfileView>(),
            };
            return View(viewModel);
        }
    }
}