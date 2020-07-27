using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Options;
using KerykeionCmsCore.Services;
using KerykeionCmsCore.PageModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : KerykeionPageModel
    {
        private readonly ISignInService _signInService;
        private readonly IUserService _userService;
        private readonly IEmailService _sendMailService;
        private readonly ILogger<RegisterModel> _logger;
        private readonly KerykeionCmsOptions _options;

        public RegisterModel(
            IUserService userService,
            ISignInService signInService,
            IEmailService sendEmailService,
            ILogger<RegisterModel> logger,
            KerykeionTranslationsService translationsService,
            IOptions<KerykeionCmsOptions> options) : base(translationsService)
        {
            _userService = userService;
            _signInService = signInService;
            _sendMailService = sendEmailService;
            _logger = logger;
            _options = options.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public bool IsAuthenticationViaExternalServicesEnabled { get; set; }

        public string ReturnUrl { get; set; }
        public string TxtSelectLanguage { get; set; }

        public class InputModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public List<PickedLanguageDto> Languages { get; set; }
            public string Language { get; set; }


            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }

            public IFormFile ProfileImage { get; set; }
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            if (!_options.Pages.IsRegisteringEnabled)
            {
                return NotFound("Deze pagina is niet beschikbaar.");
            }
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

            TxtSelectLanguage = await TranslationsService.TranslateAsync("Select a language.");
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
            IsAuthenticationViaExternalServicesEnabled = _options.User.CanUseExternalAuthenticationServices;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();

            var result = await _userService.CreateAsync(Input.Username, Input.Email, Input.Password, Input.ProfileImage, Input?.Language);
            if (result.Successfull)
            {
                var userId = await _userService.GetUserIdAsync(result.Entity);
                var roleResult = await _userService.AddToRoleAsync(result.Entity, RoleContstants.RegularUser);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
                var code = await _userService.GenerateEmailConfirmationTokenAsync(result.Entity);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId, code, returnUrl },
                    protocol: Request.Scheme);

                var sendMailResult = await _sendMailService.SendEmailAsync(Input.Username, Input.Email,
                    $"{await TranslationsService.TranslateAsync("Confirm your email")}",
                    $"{await TranslationsService.TranslateAsync("Klik")} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{await TranslationsService.TranslateAsync("hier")}</a> {await TranslationsService.TranslateAsync("pour confirmer votre compte.")}");

                if (!sendMailResult.Success)
                {
                    _logger.LogError($"Gebruiker werd succesvol geregistreerd maar kan zijn e-mail niet bevestigen want {sendMailResult.Message}.");
                }

                if (_userService.IdentityOptions().SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                }
                else
                {
                    await _signInService.SignInAsync(result.Entity, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return Page();
        }
    }
}
