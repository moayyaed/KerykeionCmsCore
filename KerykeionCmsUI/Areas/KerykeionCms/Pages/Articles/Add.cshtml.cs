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
            KerykeionArticlesService articlesService,
            KerykeionWebPagesService webPagesService) : base(translationsService, articlesService, webPagesService)
        {
        }
    }
}
