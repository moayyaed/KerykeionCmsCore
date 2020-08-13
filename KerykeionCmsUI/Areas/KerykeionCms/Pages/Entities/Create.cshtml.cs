using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Entities
{
    public class CreateModel : KerykeionPageModel
    {
        public CreateModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService) : base(translationsService, entitiesService)
        {
        }

        public string NameDisplay => TranslationsService.TranslateAsync("Name").Result;
        public string NameRequiredError => TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{NameDisplay}' is required.", NameDisplay);
        public string NameLengthError => TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{NameDisplay}' must contain a minimum of {2} and a maximum of {50} characters.", NameDisplay, 2.ToString(), 50.ToString());

        public List<IProperty> Properties { get; set; }
        [BindProperty]
        public string TableName { get; set; }

        public IActionResult OnGet(string table)
        {
            var properties = EntitiesService.GetEntityPropertiesByTable(table);
            if (properties == null)
            {
                return NotFound();
            }
            ViewData["TableName"] = table;

            if (EntitiesService.InheritsFromKeryKeionBaseClass(EntitiesService.FindEntityTypeByTableName(table)))
            {
                Properties = properties.Where(p => !p.IsPrimaryKey() && !p.Name.Equals("datetimecreated", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else Properties = properties.ToList();

            PageTitle = $"{BtnCreateValue} {EntitiesService.FindEntityTypeByTableName(table)?.ClrType?.Name}";
            TableName = table;
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            var result = await EntitiesService.CreateAsync(TableName, formDict);
            if (result.Successfull)
            {
                return RedirectToPage("/Entities/Index", new { table = TableName });
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Message);
            }

            return OnGet(TableName);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var tableName = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["table"];
            return OnGet(tableName);
        }
    }
}
