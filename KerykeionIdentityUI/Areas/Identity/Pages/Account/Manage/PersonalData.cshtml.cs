using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly IUserService _userService;

        public PersonalDataModel(
            IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}