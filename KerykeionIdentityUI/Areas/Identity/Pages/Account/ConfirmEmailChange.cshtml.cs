using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;

        public ConfirmEmailChangeModel(IUserService userService,
            ISignInService signInService)
        {
            _userService = userService;
            _signInService = signInService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Gebruiker met ID '{userId}' niet gevonden.");
            }

            var result = await _userService.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Fout wijzigen van e-mail";
                return Page();
            }

            await _signInService.RefreshSignInAsync(user);
            StatusMessage = "Bedankt om de wijziging van uw e-mail te bevestigen.";
            return Page();
        }
    }
}
