using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KerykeionCmsCore.PageModels
{
    public class StandAloneArticlePageModel : KerykeionPageModelBase<Article>
    {

        public StandAloneArticlePageModel(KerykeionTranslationsService translationsService,
            KerykeionCmsService<Article> articlesService) : base(translationsService, articlesService)
        {
        }

        public string NameDisplay => TranslationsService.TranslateAsync("Name").Result;
        public string ArticleTitleRequiredError => TranslationsService.TranslateRequiredError(NameDisplay);
        public string ArticleTitleLengthError => TranslationsService.TranslateStringLengthError(5, 50, NameDisplay);

        [BindProperty]
        public Guid ArticleId { get; set; }
        [BindProperty]
        public string MarkdownText { get; set; }
    }
}
