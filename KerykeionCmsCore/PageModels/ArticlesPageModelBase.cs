using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.PageModels
{
    public class ArticlesPageModelBase : KerykeionPageModelBase<Article>
    {
        private readonly KerykeionWebPagesService _webPagesService;
        public ArticlesPageModelBase(KerykeionTranslationsService translationsService,
            KerykeionArticlesService articlesService,
            KerykeionWebPagesService webPagesService) : base(translationsService, articlesService)
        {
            _webPagesService = webPagesService;
        }

        public string AddArticleUrl { get; set; }
        public string UpdateArticleUrl { get; set; }
        public string TxtName { get; set; }
        public string TxtAddedOn { get; set; }

        [BindProperty]
        public ArticlesViewModel Vm { get; set; }
        [BindProperty]
        public Guid? PageId { get; set; }

        public class ArticlesViewModel
        {
            public List<ArticleDto> Articles { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(Guid? pageId = null)
        {
            if (pageId != null)
            {
                PageId = pageId;
                ViewData["PageId"] = PageId;
                AddArticleUrl = "/WebPage/Articles/Add";
                UpdateArticleUrl = "/WebPage/Articles/Update";

                var page = await _webPagesService.FindByIdAllIncludedAsync(pageId);
                if (page == null)
                {
                    return NotFound();
                }

                Vm = new ArticlesViewModel
                {
                    Articles = page.Articles.OrderBy(a => a.Name).Select(a => new ArticleDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        DateTimeCreated = $"{a.DateTimeCreated?.ToShortDateString()} - ({a.DateTimeCreated?.ToShortTimeString()})"
                    }).ToList()
                };
            }
            else
            {
                AddArticleUrl = "/Articles/Add";
                UpdateArticleUrl = "/Article/Index";

                var articles = await Service.GetAll().Include(a => a.Webpage)
                                    .Include(a => a.Images)
                                    .ToListAsync();

                Vm = new ArticlesViewModel
                {
                    Articles = articles.OrderBy(a => a.Name).Select(a => new ArticleDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        DateTimeCreated = $"{a.DateTimeCreated?.ToShortDateString()} - ({a.DateTimeCreated?.ToShortTimeString()})"
                    }).ToList()
                };
            }

            PageTitle = await TranslationsService.TranslateAsync("Artikels");
            TxtName = await TranslationsService.TranslateAsync("Name");
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var article = await Service.FindByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var result = await Service.DeleteAsync(article);
            if (result.Successfull)
            {
                return RedirectToPage(new { pageId = PageId ?? null });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return Page();
        }

        public override async Task<IEnumerable<Article>> GetEntitiesToSortAsync(string pageId = null)
        {
            IEnumerable<Article> articles;
            if (string.IsNullOrEmpty(pageId))
            {
                articles = await Service.GetAll().Include(a => a.Webpage)
                                    .Include(a => a.Images)
                                    .ToListAsync();
            }
            else
            {
                var page = await _webPagesService.FindByIdAllIncludedAsync(Guid.Parse(pageId));
                articles = page.Articles;
            }

            return articles;
        }

        public override async Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<Article> sortedEntities, string pageId = null)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(a => CreateDto(a)),
                TxtUpdate = BtnDetailsValue,
                TxtDelete = BtnDeleteValue,
                UpdateUrl = string.IsNullOrEmpty(pageId) ? "Article/Index" : "WebPage/Articles/Update",
                DeleteReturnPage = string.IsNullOrEmpty(pageId) ? "Articles/Index" : "WebPage/Articles/Index",
                WebpageId = string.IsNullOrEmpty(pageId) ? "" : pageId
            });
        }

        public override object CreateDto(Article article)
        {
            return new
            {
                article.Id,
                Name = $"{article.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{article.DateTimeCreated?.ToShortDateString()} - ({article.DateTimeCreated?.ToShortTimeString()})"
            };
        }
    }
}
