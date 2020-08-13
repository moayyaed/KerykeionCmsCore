using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.WebPage.Articles
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

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var pageId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["page-id"];
            return await OnGetAsync(Guid.Parse(pageId));
        }
    }
}
