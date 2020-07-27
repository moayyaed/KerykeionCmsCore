using KerykeionCmsCore.Options;
using KerykeionCmsCore.Services;
using KerykeionCmsCore.PageModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : KerykeionPageModel
    {
        private readonly ISignInService _signInService;
        private readonly KerykeionCmsOptions _options;

        public LoginModel(ISignInService signInService,
            IOptions<KerykeionCmsOptions> options,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _signInService = signInService;
            _options = options.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool IsAuthenticationViaExternalServicesEnabled { get; set; }

        public class InputModel
        {
            public string Username { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!_options.Pages.IsLoginInEnabled)
            {
                return NotFound();
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
            IsAuthenticationViaExternalServicesEnabled = _options.User.CanUseExternalAuthenticationServices;

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInService.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, await TranslationsService.TranslateAsync("Ongeldige login poging."));
                return Page();
            }
        }
    }
}
