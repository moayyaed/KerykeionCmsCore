using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.WebPage.Articles
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class UpdateModel : ArticleUpdatePageModelBase
    {
        public UpdateModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionArticlesService articlesService) : base(translationsService, entitiesService, articlesService)
        {
        }
    }
}
