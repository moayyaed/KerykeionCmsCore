using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;

        public IndexModel(
            IUserService userService,
            ISignInService signInService)
        {
            _userService = userService;
            _signInService = signInService;
        }

        [DisplayName("Gebruikersnaam")]
        public string Username { get; set; }
        public string ProfileImageUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone(ErrorMessage = "Gelieve een gsm of telefoon nummer mee te geven.")]
            [Display(Name = "Gsm/Telefoon nummer")]
            public string PhoneNumber { get; set; }
            public List<PickedLanguageDto> Languages { get; set; }
            public string Language { get; set; }
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(dynamic user)
        {
            var userName = await _userService.GetUserNameAsync(user);
            var profileImgUrl = await _userService.GetUserProfileImageUrl(user);
            var phoneNumber = await _userService.GetPhoneNumberAsync(user);

            Username = userName;
            ProfileImageUrl = profileImgUrl;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Language = user.Language.ToString(),
                Languages = new List<PickedLanguageDto>
                {
                    new PickedLanguageDto {ShortLanguage = "NL", LongLanguage = "Nederlands"},
                    new PickedLanguageDto {ShortLanguage = "EN", LongLanguage = "English"},
                    new PickedLanguageDto {ShortLanguage = "FR", LongLanguage = "Français"},
                    new PickedLanguageDto {ShortLanguage = "DE", LongLanguage = "Deutsch"},
                }
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Het is niet gelukt om de gebruiker te laden met ID '{_userService.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Het is niet gelukt om de gebruiker te laden met ID'{_userService.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userService.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userService.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Onverwachte fout bij het proberen opslaan van de telefoonnummer.";
                    return RedirectToPage();
                }
            }

            if (Input.ProfileImage != null)
            {
                KerykeionDbResult uploadProfileImageResult = await _userService.AddProfileImage(user, Input.ProfileImage);
                if (!uploadProfileImageResult.Successfull)
                {
                    StatusMessage = "Onverwachte fout bij het proberen uploaden van de profielfoto.";
                    return RedirectToPage();
                }
            }

            var language = await _userService.GetLanguageAsync(user);
            if (Input.Language != language)
            {
                KerykeionDbResult setLanguageResult = await _userService.SetLanguageAsync(user, Input.Language);
                if (!setLanguageResult.Successfull)
                {
                    StatusMessage = "Onverwachte fout bij het proberen wijwigen van de taal.";
                    return RedirectToPage();
                }
            }

            await _signInService.RefreshSignInAsync(user);
            StatusMessage = "Uw profiel is geupdatet.";
            return RedirectToPage();
        }
    }
}
