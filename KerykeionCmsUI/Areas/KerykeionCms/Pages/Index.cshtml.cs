using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Services;
using KerykeionCmsCore.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class IndexModel : KerykeionPageModel
    {
        public IndexModel(KerykeionTranslationsService translationsService) : base(translationsService)
        {
        }

        public void OnGet()
        {
        }
    }
}
