using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionIdentityUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : KerykeionPageModel
    {
        public ResetPasswordConfirmationModel(KerykeionTranslationsService translationService) : base(translationService)
        {
        }

        public void OnGet()
        {

        }
    }
}
