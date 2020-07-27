using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly IEmailService _sendEmailService;

        public EmailModel(
            IUserService userservice,
            ISignInService signInService,
            IEmailService sendEmailService)
        {
            _userService = userservice;
            _signInService = signInService;
            _sendEmailService = sendEmailService;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Het {0} veld is verplicht.")]
            [EmailAddress(ErrorMessage = "Gelieve een e-mail adres op te geven.")]
            [Display(Name = "Nieuwe e-mail")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(dynamic user)
        {
            string email = await _userService.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userService.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userService.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userService.GetUserIdAsync(user);
                var code = await _userService.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId, email = Input.NewEmail, code },
                    protocol: Request.Scheme);

                var sendMailResult = await _sendEmailService.SendEmailAsync(
                    user.UserName,
                    Input.NewEmail,
                    "Bevestig uw e-mail",
                    $"Bevestig uw account door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> te klikken.");

                if (!sendMailResult.Success)
                {
                    ModelState.AddModelError(string.Empty, sendMailResult.Message);
                    return Page();
                }

                StatusMessage = "Bevestigings link om uw e-mail te wijzigen is verzonden. Check uw e-mail.";
                return RedirectToPage();
            }

            StatusMessage = "Uw e-mail is ongewijzigd.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userService.GetUserIdAsync(user);
            var email = await _userService.GetEmailAsync(user);
            var code = await _userService.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);
            var sendMailResult = await _sendEmailService.SendEmailAsync(
                    user.UserName,
                    email,
                    "Bevestig uw e-mail",
                    $"Bevestig uw account door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> te klikken.");

            if (!sendMailResult.Success)
            {
                ModelState.AddModelError(string.Empty, sendMailResult.Message);
                return Page();
            }
            StatusMessage = "Bevestigings mail is verzonden. Bekijk uw e-mail.";
            return RedirectToPage();
        }
    }
}
