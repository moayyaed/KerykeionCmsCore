using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            IUserService userService,
            ISignInService signInService,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userService = userService;
            _signInService = signInService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Het {0} veld is verplicht.")]
            [DataType(DataType.Password)]
            [DisplayName("Wachtwoord")]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            RequirePassword = await _userService.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            RequirePassword = await _userService.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userService.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var result = await _userService.DeleteAsync(user);
            var userId = await _userService.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInService.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId as string);

            return Redirect("~/");
        }
    }
}
