using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Users
{
    public class AddModel : KerykeionPageModel
    {
        private readonly IUserService _userService;

        public AddModel(KerykeionTranslationsService translationsService,
            IUserService userService) : base(translationsService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string TxtUserName { get; set; }
        public string TxtEmail { get; set; }
        public string TxtPassword { get; set; }
        public string TxtConfirmPassword { get; set; }
        public string TxtLanguage { get; set; }
        public string TxtProfilePicture { get; set; }
        public string TxtSelectLanguage { get; set; }
        public string TxtClickToAddProfPic { get; set; }
        public string TxtRequiredUserName { get; set; }
        public string TxtLengthUserName { get; set; }
        public string TxtRequiredEmail { get; set; }
        public string TxtRequiredPassword { get; set; }
        public string TxtCompareError { get; set; }

        public class InputModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public List<PickedLanguageDto> Languages { get; set; }
            public string Language { get; set; }


            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }

            public IFormFile ProfileImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PageTitle = await TranslationsService.TranslateAsync("Gebruikers");
            TxtUserName = await TranslationsService.TranslateAsync("Username");
            TxtEmail = await TranslationsService.TranslateAsync("Email");
            TxtLanguage = await TranslationsService.TranslateAsync("Language");
            TxtPassword = await TranslationsService.TranslateAsync("Password");
            TxtConfirmPassword = await TranslationsService.TranslateAsync("Confirm Password");
            TxtProfilePicture = await TranslationsService.TranslateAsync("Profile picture");
            TxtSelectLanguage = await TranslationsService.TranslateAsync("Select a language.");
            TxtClickToAddProfPic = await TranslationsService.TranslateAsync("Click here to add a profile picture.");
            TxtRequiredUserName = TranslationsService.TranslateRequiredError(TxtUserName);
            TxtRequiredEmail = TranslationsService.TranslateRequiredError(TxtEmail);
            TxtRequiredPassword = TranslationsService.TranslateRequiredError(TxtPassword);
            TxtLengthUserName = TranslationsService.TranslateStringLengthError(4, 50, TxtUserName);
            TxtCompareError = TranslationsService.TranslateCompareValidationError(TxtConfirmPassword, TxtPassword);

            Input = new InputModel
            {
                Languages = new List<PickedLanguageDto>
                {
                    new PickedLanguageDto {ShortLanguage = "NL", LongLanguage = "Nederlands"},
                    new PickedLanguageDto {ShortLanguage = "EN", LongLanguage = "English"},
                    new PickedLanguageDto {ShortLanguage = "FR", LongLanguage = "Français"},
                    new PickedLanguageDto {ShortLanguage = "DE", LongLanguage = "Deutsch"},
                }
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var result = await _userService.CreateAsync(Input.Username, Input.Email, Input.Password, Input.ProfileImage, Input?.Language);
            if (result.Successfull)
            {
                return RedirectToPage("/Users/Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync();
        }
    }
}
