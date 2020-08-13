using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KerykeionCmsCore.PageModels
{
    public class StandAloneArticlePageModel : KerykeionPageModelBase<Article>
    {

        public StandAloneArticlePageModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionCmsService<Article> articlesService) : base(translationsService, entitiesService, articlesService)
        {
        }

        public string NameDisplay => TranslationsService.TranslateAsync("Name").Result;
        public string ArticleTitleRequiredError => TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{NameDisplay}' is required.", NameDisplay);
        public string ArticleTitleLengthError => TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{NameDisplay}' must contain a minimum of {5} and a maximum of {50} characters.", NameDisplay, 5.ToString(), 50.ToString());

        [BindProperty]
        public Guid ArticleId { get; set; }
        [BindProperty]
        public string MarkdownText { get; set; }
    }
}
