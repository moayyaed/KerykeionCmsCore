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
    public class ForgotPasswordModel : KerykeionPageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _sendEmailService;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(IUserService userService,
            IEmailService sendEmailService,
            KerykeionTranslationsService translationsService,
            ILogger<ForgotPasswordModel> logger) : base(translationsService)
        {
            _userService = userService;
            _sendEmailService = sendEmailService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                _logger.LogInformation("User not found", null);
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            if (!(await _userService.IsEmailConfirmedAsync(user)))
            {
                _logger.LogInformation("user exists, confirm email is sent but user has not yet confirmed so cannot reset password.", null);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userService.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            SendEmailResult sendMailRslt = await _sendEmailService.SendEmailAsync(
                user.UserName,
                Input.Email,
                $"{await TranslationsService.TranslateAsync("Reset password")}",
                $"{await TranslationsService.TranslateAsync("Klik")} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{await TranslationsService.TranslateAsync("hier")}</a> {await TranslationsService.TranslateAsync("om uw wachtwoord te resetten.")}");
            if (!sendMailRslt.Success)
            {
                _logger.LogWarning(sendMailRslt.Message, null);
            }


            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
