using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.PageModels
{
    public class ArticleUpdatePageModelBase : StandAloneArticlePageModel
    {
        public ArticleUpdatePageModelBase(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionArticlesService articlesService) : base(translationsService, entitiesService, articlesService)
        {
        }

        [BindProperty]
        public Guid? PageId { get; set; }

        public string ReturnPage { get; set; }

        public List<ForeignKeyDto> ForeignKeys { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var article = await Service.GetAll().Include(a => a.Webpage).FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (article == null)
            {
                return NotFound();
            }

            PageTitle = await TranslationsService.TranslateAsync("Artikel bijwerken");
            ArticleId = id;

            Name = article?.Name;
            MarkdownText = article?.MarkdownText;

            ViewData["BaseArticleId"] = id;
            ViewData["ArticleId"] = article.Id;

            ReturnPage = "/Articles/Index";
            if (article.Webpage != null)
            {
                ViewData["PageId"] = article.Webpage.Id;
                PageId = article.Webpage.Id;
                ReturnPage = "/WebPage/Articles/Index";
            }

            ForeignKeys = Service.GetForeignKeyPropertiesToDto(article).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var article = await Service.FindByIdAsync(ArticleId.ToString());
            if (article == null)
            {
                return NotFound();
            }

            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            article.MarkdownText = MarkdownText;
            var result = await Service.UpdateAsync(article, formDict);
            if (result.Successfull)
            {
                return RedirectToPage(new { id = article.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(article.Id);
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var article = await Service.FindByIdAsync(ArticleId.ToString());
            if (article == null)
            {
                return NotFound();
            }

            var result = await Service.DeleteAsync(article);
            if (result.Successfull)
            {
                if (PageId == null)
                {
                    return RedirectToPage("/Articles/Index");
                }

                return RedirectToPage("/WebPage/Articles/Index", new { pageId = PageId });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(article.Id);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var articleId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["article-id"];
            return await OnGetAsync(Guid.Parse(articleId));
        }
    }
}
