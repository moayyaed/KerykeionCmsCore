using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Entities
{
    public class UpdateModel : KerykeionPageModel
    {
        private readonly EntitiesService _entitiesService;

        public UpdateModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService) : base(translationsService)
        {
            _entitiesService = entitiesService;
        }

        public string NameDisplay => TranslationsService.TranslateAsync("Name").Result;
        public string NameRequiredError => TranslationsService.TranslateRequiredError(NameDisplay);
        public string NameLengthError => TranslationsService.TranslateStringLengthError(2, 50, NameDisplay);

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public string TableName { get; set; }
        [BindProperty]
        public Guid EntityId { get; set; }
        public object Entity { get; set; }
        public List<IProperty> Properties { get; set; }

        public List<ForeignKeyDto> ForeignKeys { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id, string table)
        {
            var entity = await _entitiesService.FindByIdAndTableNameAsync(id, table);
            if (entity == null)
            {
                return NotFound();
            }

            var props = _entitiesService.GetEntityPropertiesByTable(table);
            if (props == null)
            {
                return NotFound();
            }

            ForeignKeys = _entitiesService.GetForeignKeyPropertiesToDto(entity).ToList();

            ViewData["EntityId"] = id;
            ViewData["TableName"] = table;

            TableName = table;
            Entity = entity;
            EntityId = id;
            Properties = props.ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var entity = await _entitiesService.FindByIdAndTableNameAsync(EntityId, TableName);
            if (entity == null)
            {
                return NotFound();
            }

            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);

            var result = await _entitiesService.UpdateAsync(entity, formDict);
            if (result.Successfull)
            {
                StatusMessage = $"The {entity.GetType().Name} has been succesfully updated.";
                return RedirectToPage(new { id = EntityId, table = TableName });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(EntityId, TableName);
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var entity = await _entitiesService.FindByIdAndTableNameAsync(EntityId, TableName);
            if (entity == null)
            {
                return NotFound();
            }

            var result = await _entitiesService.DeleteAsync(entity);
            if (result.Successfull)
            {
                return RedirectToPage("/Entities/Index", new { table = TableName });
            }

            foreach (var error in result.Errors)
            {
                StatusMessage += $"Error: {error}{Environment.NewLine}";
            }

            return await OnGetAsync(EntityId, TableName);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString());
            return await OnGetAsync(Guid.Parse(formDict["entity-id"]), formDict["table"]);
        }
    }
}
