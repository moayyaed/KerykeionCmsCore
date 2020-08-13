using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Articles
{
    public class IndexModel : ArticlesPageModelBase
    {
        public IndexModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionArticlesService articlesService,
            KerykeionWebPagesService webPagesService) : base(translationsService, entitiesService, articlesService, webPagesService)
        {

        }
    }
}
