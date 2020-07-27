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
    public class AddModel : KerykeionPageModel
    {
        private readonly EntitiesService _entitiesService;

        public AddModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService) : base(translationsService)
        {
            _entitiesService = entitiesService;
        }

        public string NameDisplay => TranslationsService.TranslateAsync("Name").Result;
        public string NameRequiredError => TranslationsService.TranslateRequiredError(NameDisplay);
        public string NameLengthError => TranslationsService.TranslateStringLengthError(2, 50, NameDisplay);

        public List<IProperty> Properties { get; set; }
        [BindProperty]
        public string TableName { get; set; }

        public IActionResult OnGet(string table)
        {
            var properties = _entitiesService.GetEntityPropertiesByTable(table);
            if (properties == null)
            {
                return NotFound();
            }
            ViewData["TableName"] = table;
            Properties = properties.Where(p => !p.IsPrimaryKey() && !p.Name.Equals("datetimecreated", StringComparison.OrdinalIgnoreCase)).ToList();

            TableName = table;
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            var result = await _entitiesService.AddAsync(TableName, formDict);
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
