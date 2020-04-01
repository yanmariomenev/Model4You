namespace Model4You.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Model4You.Data.Models;
    using Model4You.Services;
    using Model4You.Services.Cloudinary;
    using Model4You.Services.Data.ModelService;
    using Model4You.Web.ViewModels.BindingModels;
    using Model4You.Web.ViewModels.Model;
    using Model4You.Web.ViewModels.ModelViews;

    public class ModelsController : Controller
    {
        public const int ProfilesPerPage = 6;
        private readonly IModelService modelService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public ModelsController(
            IModelService modelService,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.modelService = modelService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
        }

        // GET
        public async Task<IActionResult> Model(int page = 1, int perPage = ProfilesPerPage)
        {
            var pagesCount = await this.modelService.GetPagesCount(perPage);
            var viewModel = new IndexProfileViewModel
            {
                ModelProfile =
                   await this.modelService.TakeAllModels<ModelProfileView>(page, perPage),
                CurrentPage = page,
                PagesCount = pagesCount,
            };
            return this.View(viewModel);
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

        [Authorize]
        public async Task<IActionResult> Album()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pictures = await this.modelService.TakeAllPictures<AlbumViewModel>(userId);
            var viewModel = new AlbumBindingViewModel
            {
                AlbumViewModel = pictures,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Album(AlbumBindingViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // TODO Limit users to upload only 6-8 pictures
            var imageUrls = input.AlbumInputViewModel.UserImages
                .Select(async x =>
                    await cloudinaryService.UploadPictureAsync(x, x.FileName))
                .Select(x => x.Result)
                .ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.modelService.UploadAlbum(imageUrls, userId);

            // TODO redirect to user profile.
            return this.Redirect("/Models/Profile/" + userId);
        }
    }
}