using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Help
{
    public class IndexModel : KerykeionPageModel
    {
        public IndexModel(KerykeionTranslationsService translationsService) : base(translationsService)
        {
        }

        public string TxtGettingStarted { get; set; }
        public string TxtGettingStartedAddEntities { get; set; }
        public string TxtGettingStartedUpdateEntities { get; set; }
        public string TxtGettingStartedDeleteEntities { get; set; }
        public string TxtExtendKerykeionUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TxtGettingStarted = await TranslationsService.TranslateAsync("Getting started");
            TxtGettingStartedAddEntities = $"{TxtGettingStarted}.. ({BtnAddValue})";
            TxtGettingStartedUpdateEntities = $"{TxtGettingStarted}.. ({BtnUpdateValue})";
            TxtGettingStartedDeleteEntities = $"{TxtGettingStarted}.. ({BtnDeleteValue})"; ;
            TxtExtendKerykeionUser = await TranslationsService.TranslateAsync("Extend KerykeionUser");

            return Page();
        }
    }
}
