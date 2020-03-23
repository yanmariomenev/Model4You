namespace Model4You.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.ModelService;
    using Model4You.Web.ViewModels.Model;
    using Model4You.Web.ViewModels.ModelViews;

    public class ModelsController : Controller
    {
        private readonly IModelService modelService;

        public ModelsController(IModelService modelService)
        {
            this.modelService = modelService;
        }

        // GET
        public async Task<IActionResult> Model()
        {
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                   await this.modelService.TakeAllModels<ModelProfileView>(),
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Profile(string id)
        {
            var viewModel = await this.modelService
                .GetModelById<ProfileViewModel>(id);

            // Get the display name of the enum. TODO Try find a better way
            var displayName = viewModel.ModelInformation.Ethnicity.GetDisplayName();

            // Using viewData so i don't use the service in the html file.
            this.ViewData["Ethnicity"] = displayName;

            return this.View(viewModel);
        }

    }
}