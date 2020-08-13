using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.PageModels
{
    public class ArticleAddPageModelBase : StandAloneArticlePageModel
    {
        private readonly KerykeionWebPagesService _webPagesService;

        public ArticleAddPageModelBase(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionArticlesService articlesService,
            KerykeionWebPagesService webPagesService) : base(translationsService, entitiesService, articlesService)
        {
            _webPagesService = webPagesService;
        }

        [BindProperty]
        public Guid? PageId { get; set; }
        public string ReturnToArticlesUrl { get; set; }
        public class ArticleForeignKeysVm
        {
            public List<string> ForeignKeyPropertyNames { get; set; }
        }
        public ArticleForeignKeysVm Vm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? pageId = null)
        {
            if (pageId != null)
            {
                PageId = pageId;
                ViewData["PageId"] = pageId;
                ReturnToArticlesUrl = "/WebPage/Articles/Index";
            }
            else
            {
                ReturnToArticlesUrl = "/Articles/Index";
                Vm = new ArticleForeignKeysVm
                {
                    ForeignKeyPropertyNames = Service.GetForeignKeyProperties()
                                                .Select(fk => fk.Name).ToList()
                };
            }


            PageTitle = await TranslationsService.TranslateAsync("Artikel toevoegen");
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            Webpage page = await _webPagesService.FindByIdAllIncludedAsync(PageId);

            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            var addArticleResult = await Service.CreateAsync(formDict);
            if (addArticleResult.Successfull)
            {
                if (page == null)
                {
                    return RedirectToPage("/Articles/Index");
                }

                var addArticleToPageResult = await _webPagesService.AddArticleAsync(page, addArticleResult.Entity as Article);
                if (addArticleToPageResult.Successfull)
                {
                    return RedirectToPage("/WebPage/Articles/Index", new { pageId = page.Id });
                }

                foreach (var error in addArticleToPageResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            foreach (var error in addArticleResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(PageId ?? null);
        }
    }
}
