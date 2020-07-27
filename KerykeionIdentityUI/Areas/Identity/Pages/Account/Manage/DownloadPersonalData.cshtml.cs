using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            IUserService userService,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userService.GetUserId(User)}'.");
            }
            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userService.GetUserId(User));

            var userType = user.GetType() as Type;

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = userType.GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userService.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersoonlijkeGegevens.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}
