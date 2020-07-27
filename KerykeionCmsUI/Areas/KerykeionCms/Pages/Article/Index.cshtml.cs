using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Article
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class IndexModel : ArticleUpdatePageModelBase
    {
        public IndexModel(KerykeionTranslationsService translationsService,
            KerykeionArticlesService articlesService) : base(translationsService, articlesService)
        {
        }
    }
}
