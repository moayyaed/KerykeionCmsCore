using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Help
{
    public class ExceptionModel : KerykeionPageModel
    {
        public ExceptionModel(KerykeionTranslationsService translationsService) : base(translationsService)
        {
        }

        public string Documentation { get; set; }
        [BindProperty]
        public int DocId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var documentation = await TranslationsService.FindByExcDoxIdAsync(id);
            if (documentation == null)
            {
                return NotFound();
            }

            PageTitle = "Help desk";
            Documentation = documentation;
            DocId = id;

            return Page();
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var docId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["doc-id"];
            return await OnGetAsync(int.Parse(docId));
        }
    }
}
