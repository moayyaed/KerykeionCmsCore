using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;

        public SetPasswordModel(
            IUserService userService,
            ISignInService signInService)
        {
            _userService = userService;
            _signInService = signInService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Het {0} veld is verplicht.")]
            [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} en maximum {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nieuw wachtwoord")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord en bevesting wachtwoord komen niet overeen.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            var hasPassword = await _userService.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToPage("./ChangePassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userService.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInService.RefreshSignInAsync(user);
            StatusMessage = "Uw wachtwoord is geplaatst.";

            return RedirectToPage();
        }
    }
}
