using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model4You.Data.Models;
using Model4You.Data.Models.Enums;
using Model4You.Services.Data.ModelService;
using Model4You.Web.ViewModels.Model;

namespace Model4You.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IModelService _modelService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IModelService modelService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _modelService = modelService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Display(Name = "Age")]
            public int Age { get; set; }

            [Display(Name = "Select Gender")]
            public Gender Gender { get; set; }

            [Display(Name = "Select Ethnicity")]
            public Ethnicity Ethnicity { get; set; }

            [Display(Name = "Your Height")]
            public double Height { get; set; }

            [Display(Name = "Bust size")]
            public double Bust { get; set; }

            [Display(Name = "Waist size")]
            public double Waist { get; set; }

            [Display(Name = "Hips size")]
            public double Hips { get; set; }

            [Display(Name = "Type of modeling commercial, Swimsuit, fit and etc.")]
            public string ModelType { get; set; }

            [Display(Name = "Link your Instagram")]
            public string InstagramUrl { get; set; }

            [Display(Name = "Link your Facebook")]
            public string FacebookUrl { get; set; }

            [Display(Name = "Nationality")]
            public string Nationality { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            string userId = await _userManager.GetUserIdAsync(user);
            var model = await _modelService.GetModelById<ProfileViewModel>(userId);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.ModelInformation.Age,
                Gender = model.ModelInformation.Gender,
                Height = model.ModelInformation.Height,
                Bust = model.ModelInformation.Bust,
                Hips = model.ModelInformation.Hips,
                Waist = model.ModelInformation.Waist,
                Ethnicity = model.ModelInformation.Ethnicity,
                ModelType = model.ModelInformation.ModelType,
                Nationality = model.ModelInformation.Nationality,
                FacebookUrl = model.ModelInformation.FacebookUrl,
                InstagramUrl = model.ModelInformation.InstagramUrl,
            };

            var test = new InputModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
               await _modelService.ChangeUserFirstName(user, Input.FirstName);
            }

            if (Input.LastName != user.LastName)
            {
                await _modelService.ChangeUserLastName(user, Input.LastName);
            }
            string currentUserId = await _userManager.GetUserIdAsync(user);
            var modelInformation = await _modelService.GetModelById<ChangeUserInformationInputModel>(currentUserId);

            if (Input.Age != user.ModelInformation.Age)
            {
                await _modelService.ChangeUserAge(user, Input.Age);
            }

            if (Input.Gender != user.ModelInformation.Gender)
            {
                await _modelService.ChangeUserGender(user, Input.Gender);
            }

            if (Input.Ethnicity != user.ModelInformation.Ethnicity)
            {
                await _modelService.ChangeUserEthnicity(user, Input.Ethnicity);
            }

            if (Input.Height != user.ModelInformation.Height)
            {
                await _modelService.ChangeUserValues(user, Input.Height, "height");
            }

            if (Input.Height != user.ModelInformation.Waist)
            {
                await _modelService.ChangeUserValues(user, Input.Waist, "waist");
            }

            if (Input.Height != user.ModelInformation.Bust)
            {
                await _modelService.ChangeUserValues(user, Input.Bust, "bust");
            }

            if (Input.Height != user.ModelInformation.Hips)
            {
                await _modelService.ChangeUserValues(user, Input.Hips, "hips");
            }

            if (Input.ModelType != user.ModelInformation.ModelType)
            {
                await _modelService.ChangeUserStringValues(user,Input.ModelType, "modelType");
            }

            if (Input.Nationality != user.ModelInformation.Nationality)
            {
                await _modelService.ChangeUserStringValues(user, Input.Nationality, "nationality");
            }

            if (Input.InstagramUrl != user.ModelInformation.InstagramUrl)
            {
                await _modelService.ChangeUserStringValues(user, Input.InstagramUrl, "instagramUrl");
            }

            if (Input.FacebookUrl != user.ModelInformation.FacebookUrl)
            {
                await _modelService.ChangeUserStringValues(user, Input.FacebookUrl, "facebookUrl");
            }

            await _signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
