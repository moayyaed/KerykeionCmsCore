using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KerykeionLoginUI.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ISignInService _signInService;

        public LogoutModel(ISignInService signInService)
        {
            _signInService = signInService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInService.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
