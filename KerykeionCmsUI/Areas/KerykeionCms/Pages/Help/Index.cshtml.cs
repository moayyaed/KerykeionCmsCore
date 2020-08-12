using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Help
{
    public class IndexModel : KerykeionPageModel
    {
        public IndexModel(KerykeionTranslationsService translationsService) : base(translationsService)
        {
        }

        public string TxtGettingStarted { get; set; }
        public string TxtGettingStartedCreateEntities { get; set; }
        public string TxtGettingStartedReadEntitiesPartOne { get; set; }
        public string TxtGettingStartedReadEntitiesPartTwo { get; set; }
        public string TxtGettingStartedUpdateEntities { get; set; }
        public string TxtGettingStartedDeleteEntities { get; set; }
        public string TxtCreateArticles { get; set; }
        public string TxtCreateWebpages { get; set; }
        public string TxtCreateLinks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TxtGettingStarted = await TranslationsService.TranslateAsync("Getting started");
            TxtGettingStartedCreateEntities = $"{TxtGettingStarted}.. ({BtnCreateValue})";
            TxtGettingStartedReadEntitiesPartOne = $"{TxtGettingStarted}.. ({await TranslationsService.TranslateAsync("Read")}) {await TranslationsService.TranslateAsync("part one")}.";
            TxtGettingStartedReadEntitiesPartTwo = $"{TxtGettingStarted}.. ({await TranslationsService.TranslateAsync("Read")}) {await TranslationsService.TranslateAsync("part two")}.";
            TxtGettingStartedUpdateEntities = $"{TxtGettingStarted}.. ({BtnUpdateValue})";
            TxtGettingStartedDeleteEntities = $"{TxtGettingStarted}.. ({BtnDeleteValue})";
            TxtCreateArticles = await TranslationsService.TranslateAsync("Create articles");
            TxtCreateWebpages = await TranslationsService.TranslateAsync("Create webpages");
            TxtCreateLinks= await TranslationsService.TranslateAsync("Create links");

            return Page();
        }
    }
}
