using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Enums;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Entities
{
    public class IndexModel : KerykeionPageModel
    {
        private readonly EntitiesService _entitiesService;
        public IndexModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService) : base(translationsService)
        {
            _entitiesService = entitiesService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<EntitySideNavDto> Entities { get; set; }
        public string TxtName { get; set; }
        public string TxtAddedOn { get; set; }

        [BindProperty]
        public string TableName { get; set; }

        public async Task<IActionResult> OnGetAsync(string table)
        {
            var entities = await _entitiesService.ListAllToDtoAsync(table);
            if (entities == null)
            {
                return NotFound();
            }

            ViewData["TableName"] = table;
            Entities = entities.ToList();
            TableName = table;
            TxtName = await TranslationsService.TranslateAsync("Name");
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var entity = await _entitiesService.FindByIdAndTableNameAsync(id, TableName);
            if (entity == null)
            {
                return NotFound();
            }

            var result = await _entitiesService.DeleteAsync(entity);
            if (result.Successfull)
            {
                StatusMessage = "The entity has been successfully deleted.";
                return RedirectToPage(new { table = TableName });
            }

            foreach (var error in result.Errors)
            {
                StatusMessage += $"Error: {error.Message}<br />";
            }

            return await OnGetAsync(TableName);
        }

        public virtual async Task<IActionResult> OnPostSortNameAsync([FromBody] SortDto dto)
        {
            IEnumerable<object> entities = await GetEntitiesToSortAsync(dto.Table);
            var sortOrder = Enum.Parse<KerykeionCmsSortingOrder>(dto.SortingOrder);
            List<KerykeionBaseClass> sortedEntities = sortOrder switch
            {
                KerykeionCmsSortingOrder.Ascending => entities.Cast<KerykeionBaseClass>().OrderByDescending(a => a.UniqueNameIdentifier).ToList(),
                KerykeionCmsSortingOrder.Descending => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.UniqueNameIdentifier).ToList(),
                KerykeionCmsSortingOrder.None => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.UniqueNameIdentifier).ToList(),
                _ => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.UniqueNameIdentifier).ToList(),
            };

            return await AnonymousObjectWithSortedEntitiesAsync(sortedEntities, dto.Table);
        }

        public virtual async Task<IActionResult> OnPostSortDateTimeAsync([FromBody] SortDto dto)
        {
            IEnumerable<object> entities = await GetEntitiesToSortAsync(dto.Table);
            var sortOrder = Enum.Parse<KerykeionCmsSortingOrder>(dto.SortingOrder);
            List<KerykeionBaseClass> sortedEntities = sortOrder switch
            {
                KerykeionCmsSortingOrder.Ascending => entities.Cast<KerykeionBaseClass>().OrderByDescending(a => a.DateTimeCreated).ToList(),
                KerykeionCmsSortingOrder.Descending => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.DateTimeCreated).ToList(),
                KerykeionCmsSortingOrder.None => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.DateTimeCreated).ToList(),
                _ => entities.Cast<KerykeionBaseClass>().OrderBy(a => a.DateTimeCreated).ToList(),
            };

            return await AnonymousObjectWithSortedEntitiesAsync(sortedEntities, dto.Table);
        }

        public virtual async Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<KerykeionBaseClass> sortedEntities, string table)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(e => CreateDto(e)),
                TxtUpdate = BtnDetailsValue,
                TxtDelete = BtnDeleteValue,
                UpdateUrl = $"Entities/Update",
                Table = table,
                DeleteReturnPage = "Entities",
            });
        }

        public virtual object CreateDto(object entity)
        {
            var e = entity as KerykeionBaseClass;

            return new
            {
                e.Id,
                Name = $"{e.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{e.DateTimeCreated?.ToShortDateString()} - ({e.DateTimeCreated?.ToShortTimeString()})"
            };
        }

        public virtual async Task<IEnumerable<object>> GetEntitiesToSortAsync(string table)
        {
            return await _entitiesService.ListAllAsync(table);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var tableName = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["table"];
            return await OnGetAsync(tableName);
        }
    }
}
