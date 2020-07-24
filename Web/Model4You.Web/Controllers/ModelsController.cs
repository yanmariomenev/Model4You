namespace Model4You.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Model4You.Data.Models;
    using Model4You.Services;
    using Model4You.Services.Cloudinary;
    using Model4You.Services.Data.ImageService;
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
        private readonly IImageService imageService;

        public ModelsController(
            IModelService modelService,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager,
            IImageService imageService)
        {
            this.modelService = modelService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
            this.imageService = imageService;
        }

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

            if (viewModel == null)
            {
                return this.NotFound();
            }

            // Get the display name of the enum. TODO Try find a better way.
            var displayName = viewModel.ModelInformation.Ethnicity.GetDisplayName();

            // Using viewData so i don't use the service in the html file.
            this.ViewData["Ethnicity"] = displayName;
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = await this.modelService.GetModelById<ProfileViewModel>(userId);
            var modelInformation = viewModel.ModelInformation;
            if (modelInformation == null)
            {
                return this.Redirect("/Identity/Account/Manage");
            }

            return this.RedirectToAction("Profile", "Models", new { @id = userId });
        }

        [Authorize]
        public async Task<IActionResult> Album()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var imageCount = await this.imageService.GetImageCountOfCurrentUser(userId);
            var pictures = await this.modelService.TakeAllPictures<AlbumViewModel>(userId);

            var viewModel = new AlbumBindingViewModel
            {
                AlbumViewModel = pictures,
                ImageCount = imageCount,
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

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var imageUrls = input.AlbumInputViewModel.UserImages
                .Select(async x =>
                    await this.cloudinaryService.UploadPictureAsync(x, x.FileName))
                .Select(x => x.Result)
                .ToList();
            await this.modelService.UploadAlbum(imageUrls, userId);

            return this.RedirectToAction(nameof(this.MyProfile));
        }

        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var imageToDelete = await this.imageService.DeleteImage(id);
            return this.RedirectToAction(nameof(this.Album));
        }

        [Authorize]
        public async Task<IActionResult> DeleteProfilePicture(string id)
        {
            var imageToDelete = await this.imageService.DeleteProfilePicture(id);

            return this.RedirectToAction(nameof(this.Album));
        }

        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture(string imageUrl, string userId, int imageId)
        {
            var changeProfilePicture = await this.imageService
                .ChangeProfilePicture(imageUrl, userId, imageId);

            return this.RedirectToAction(nameof(this.Album));
        }

    }
}