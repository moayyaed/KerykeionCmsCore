using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KerykeionCmsCore.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;
using KerykeionCmsCore.Dtos;

namespace KerykeionCmsCore.PageModels
{
    public class KerykeionPageModel : PageModel
    {
        protected readonly KerykeionTranslationsService TranslationsService;
        protected readonly EntitiesService EntitiesService;
        public KerykeionPageModel(KerykeionTranslationsService translationsService, 
            EntitiesService entitiesService)
        {
            TranslationsService = translationsService;
            EntitiesService = entitiesService;
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

        public async Task<JsonResult> OnPostDeleteViaAjaxAsync([FromBody] DeleteEntityFromSideNavDto dto)
        {
            var entity = await EntitiesService.FindByIdAndTableNameAsync(dto.Id, dto.Table);
            if (entity == null)
            {
                return new JsonResult(KerykeionDbResult.Fail(new KerykeionDbError { Message = "The entity is not found."}));
            }

            var result = await EntitiesService.DeleteAsync(entity);

            return new JsonResult(result);
        }

        public async Task SetLanguageAsync()
        {
            await Task.Delay(0);
            var language = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["chosen-language"];
            TranslationsService.Options.Pages.Language = Enum.Parse<KerykeionCmsLanguage>(language);
        }
    }
}
