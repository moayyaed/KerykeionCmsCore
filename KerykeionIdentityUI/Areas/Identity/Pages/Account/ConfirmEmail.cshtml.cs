using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly KerykeionTranslationsService _translationsService;

        public ConfirmEmailModel(IUserService userService,
            KerykeionTranslationsService translationsService)
        {
            _userService = userService;
            _translationsService = translationsService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userService.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? $"{await _translationsService.TranslateAsync("Bedankt om uw e-mail te bevestigen.")}" : $"{await _translationsService.TranslateAsync("Het bevestigen van uw e-mail is mislukt.")}";
            return Page();
        }
    }
}
