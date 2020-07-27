using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly ILogger<ResetAuthenticatorModel> _logger;

        public ResetAuthenticatorModel(
            IUserService userService,
            ISignInService signInService,
            ILogger<ResetAuthenticatorModel> logger)
        {
            _userService = userService;
            _signInService = signInService;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            await _userService.SetTwoFactorEnabledAsync(user, false);
            await _userService.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id as string);

            await _signInService.RefreshSignInAsync(user);
            StatusMessage = "Uw authenticator app werd gereset, u zult moeten uw authenticator app configureren met de nieuwe sleutel.";

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}