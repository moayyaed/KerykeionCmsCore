using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Articles
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class AddModel : ArticleAddPageModelBase
    {
        public AddModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionArticlesService articlesService,
            KerykeionWebPagesService webPagesService) : base(translationsService, entitiesService, articlesService, webPagesService)
        {
        }
    }
}
