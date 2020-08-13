using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Services;
using KerykeionCmsCore.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class IndexModel : KerykeionPageModel
    {
        public IndexModel(KerykeionTranslationsService translationsService, 
            EntitiesService entitiesService) : base(translationsService, entitiesService)
        {
        }

        public void OnGet()
        {
        }
    }
}
