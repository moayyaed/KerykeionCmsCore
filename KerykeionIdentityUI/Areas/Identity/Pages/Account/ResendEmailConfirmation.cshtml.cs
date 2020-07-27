using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : KerykeionPageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _sendMailService;
        private readonly ILogger<ResendEmailConfirmationModel> _logger;

        public ResendEmailConfirmationModel(IUserService userService,
            IEmailService sendEmailService,
            KerykeionTranslationsService translationsService,
            ILogger<ResendEmailConfirmationModel> logger) : base(translationsService)
        {
            _userService = userService;
            _sendMailService = sendEmailService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return Page();
            }

            var userId = await _userService.GetUserIdAsync(user);
            var code = await _userService.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);
            SendEmailResult result = await _sendMailService.SendEmailAsync(
                user.UserName,
                Input.Email,
                $"{await TranslationsService.TranslateAsync("Confirm your email")}",
                $"{await TranslationsService.TranslateAsync("Klik")} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{await TranslationsService.TranslateAsync("hier")}</a> {await TranslationsService.TranslateAsync("pour confirmer votre compte.")}");

            if (!result.Success)
            {
                _logger.LogWarning(result.Message, null);
            }

            return RedirectToPage("/Index");
        }
    }
}
