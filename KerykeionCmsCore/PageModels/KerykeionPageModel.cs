using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KerykeionCmsCore.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace KerykeionCmsCore.PageModels
{
    public class KerykeionPageModel : PageModel
    {
        protected readonly KerykeionTranslationsService TranslationsService;
        public KerykeionPageModel(KerykeionTranslationsService translationsService)
        {
            TranslationsService = translationsService;
        }

        public string PageTitle { get; set; }
        public string BtnCreateValue => TranslationsService.TranslateAsync("Create").Result;
        public string BtnDeleteValue => TranslationsService.TranslateAsync("Delete").Result;
        public string BtnBackValue => TranslationsService.TranslateAsync("Back").Result;
        public string BtnUpdateValue => TranslationsService.TranslateAsync("Update").Result;
        public string BtnSearchValue => TranslationsService.TranslateAsync("Search").Result;
        public string BtnDetailsValue => TranslationsService.TranslateAsync("Details").Result;

        public virtual async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            return RedirectToPage();
        }

        public async Task SetLanguageAsync()
        {
            await Task.Delay(0);
            var language = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["chosen-language"];
            TranslationsService.Options.Pages.Language = Enum.Parse<KerykeionCmsLanguage>(language);
        }
    }
}
