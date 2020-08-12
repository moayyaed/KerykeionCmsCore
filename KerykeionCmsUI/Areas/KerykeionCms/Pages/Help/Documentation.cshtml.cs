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
        public string ReturnId { get; set; }
        public string Documentation { get; set; }
        public string ReturnUrl { get; set; } = "/Help/Documentation";

        public async Task<IActionResult> OnGetAsync(Guid id, string returnId = null)
        {
            var documentation = await TranslationsService.FindDocByIdAsync(id);
            if (documentation == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(returnId))
            {
                ReturnUrl = "/Help/Index";
            }

            ReturnId = returnId;
            DocId = id;
            Documentation = documentation;

            return Page();
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var docId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["doc-id"];
            var returnId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["return-id"];
            return await OnGetAsync(Guid.Parse(docId), returnId);
        }
    }
}
