using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionLoginUI.Pages
{
    public class LoginModel : KerykeionPageModel
    {
        private readonly ISignInService _signInService;

        public LoginModel(KerykeionTranslationsService translationsService,
            ISignInService singnInService) : base(translationsService)
        {
            _signInService = singnInService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public string TxtUserName { get; set; }
        public string TxtPassword { get; set; }
        public string TxtRememberMe { get; set; }
        public string TxtRequiredUserName { get; set; }
        public string TxtRequiredPassword { get; set; }

        public class InputModel
        {
            public string Username { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            PageTitle = await TranslationsService.TranslateAsync("inloggen");
            TxtUserName = await TranslationsService.TranslateAsync("username");
            TxtPassword = await TranslationsService.TranslateAsync("password");
            TxtRememberMe = await TranslationsService.TranslateAsync("Remember me");
            TxtRequiredPassword = TranslationsService.TranslateRequiredError(TxtPassword);
            TxtRequiredUserName = TranslationsService.TranslateRequiredError(TxtUserName);

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var result = await _signInService.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, await TranslationsService.TranslateAsync("Ongeldige login poging."));
            return await OnGetAsync();
        }
    }
}
