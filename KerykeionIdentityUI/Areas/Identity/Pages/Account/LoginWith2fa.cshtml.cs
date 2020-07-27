using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(ISignInService signInService, ILogger<LoginWith2faModel> logger)
        {
            _signInService = signInService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "De {0} moet minstens {2} en maximum {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Onthoud deze machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInService.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Niet in staat om twee facor authenticatie gebruiker te laden.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl ??= Url.Content("~/");

            var user = await _signInService.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Niet in staat om twee facor authenticatie gebruiker te laden.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInService.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id as string);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id as string);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id as string);
                ModelState.AddModelError(string.Empty, "Ongeldige authenticator code.");
                return Page();
            }
        }
    }
}
