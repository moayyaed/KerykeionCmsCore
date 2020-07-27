using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            IUserService userService,
            ILogger<Disable2faModel> logger)
        {
            _userService = userService;
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

            if (!await _userService.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Cannot disable 2FA for user with ID '{_userService.GetUserId(User)}' as it's not currently enabled.");
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

            var disable2faResult = await _userService.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred disabling 2FA for user with ID '{_userService.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userService.GetUserId(User));
            StatusMessage = "2fa werd uitgeschakeld. U kunt 2fa herinschakelen wanneer een een authenticator app opzet.";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}