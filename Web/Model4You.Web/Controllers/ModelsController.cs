﻿using Model4You.Web.ViewModels.Model;

namespace Model4You.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data.ModelService;
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

            return this.View(viewModel);
        }
    }
}