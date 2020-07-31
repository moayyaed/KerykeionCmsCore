using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Help
{
    public class DocumentationModel : KerykeionPageModel
    {
        public DocumentationModel(KerykeionTranslationsService translationsService) : base(translationsService)
        {
        }

        public Guid DocId { get; set; }
        public string Documentation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var documentation = await TranslationsService.FindByIdAsync(id);
            if (documentation == null)
            {
                return NotFound();
            }

            DocId = id;

            Documentation = documentation;

            return Page();
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var docId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["doc-id"];
            return await OnGetAsync(Guid.Parse(docId));
        }
    }
}
