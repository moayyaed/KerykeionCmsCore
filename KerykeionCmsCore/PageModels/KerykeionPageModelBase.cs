using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Enums;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.PageModels
{
    public class KerykeionPageModelBase<TEntity> : KerykeionPageModel
        where TEntity : KerykeionBaseClass
    {
        protected readonly KerykeionCmsService<TEntity> Service;
        public KerykeionPageModelBase(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionCmsService<TEntity> service) : base(translationsService, entitiesService)
        {
            Service = service;
        }

        [PageRemote(PageHandler = "VerifyNotExists", AdditionalFields = "__RequestVerificationToken", HttpMethod = "Post")]
        [BindProperty]
        public string Name { get; set; }

        public virtual async Task<IActionResult> OnPostSortNameAsync([FromBody] SortDto dto)
        {
            IEnumerable<TEntity> entities = await GetEntitiesToSortAsync(dto.PageId);
            var sortOrder = Enum.Parse<KerykeionCmsSortingOrder>(dto.SortingOrder);
            List<TEntity> sortedEntities = sortOrder switch
            {
                KerykeionCmsSortingOrder.Ascending => entities.OrderByDescending(a => a.UniqueNameIdentifier).ToList(),
                KerykeionCmsSortingOrder.Descending => entities.OrderBy(a => a.UniqueNameIdentifier).ToList(),
                KerykeionCmsSortingOrder.None => entities.OrderBy(a => a.UniqueNameIdentifier).ToList(),
                _ => entities.OrderBy(a => a.UniqueNameIdentifier).ToList(),
            };

            return await AnonymousObjectWithSortedEntitiesAsync(sortedEntities, dto.PageId);
        }

        public virtual async Task<IActionResult> OnPostSortDateTimeAsync([FromBody] SortDto dto)
        {
            IEnumerable<TEntity> entities = await GetEntitiesToSortAsync(dto.PageId);
            var sortOrder = Enum.Parse<KerykeionCmsSortingOrder>(dto.SortingOrder);
            List<TEntity> sortedEntities = sortOrder switch
            {
                KerykeionCmsSortingOrder.Ascending => entities.OrderByDescending(a => a.DateTimeCreated).ToList(),
                KerykeionCmsSortingOrder.Descending => entities.OrderBy(a => a.DateTimeCreated).ToList(),
                KerykeionCmsSortingOrder.None => entities.OrderBy(a => a.DateTimeCreated).ToList(),
                _ => entities.OrderBy(a => a.DateTimeCreated).ToList(),
            };

            return await AnonymousObjectWithSortedEntitiesAsync(sortedEntities, dto.PageId);
        }

        public virtual async Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<TEntity> sortedEntities, string pageId = null)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(e => CreateDto(e)),
                TxtUpdate = BtnDetailsValue,
                TxtDelete = BtnDeleteValue,
                UpdateUrl = "",
                DeleteReturnPage = "",
                WebpageId = string.IsNullOrEmpty(pageId) ? "" : pageId
            });
        }

        public virtual object CreateDto(TEntity e)
        {
            return new
            {
                e.Id,
                Name = $"{e.UniqueNameIdentifier.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{e.DateTimeCreated?.ToShortDateString()} - ({e.DateTimeCreated?.ToShortTimeString()})"
            };
        }

        public virtual async Task<IEnumerable<TEntity>> GetEntitiesToSortAsync(string pageId = null)
        {
            return await Service.ListAllAsync();
        }

        public async Task<IActionResult> OnPostVerifyNotExistsAsync()
        {
            var exists = await Service.ExistsAsync(Name);
            if (exists)
            {
                return new JsonResult(TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NameDuplicate, $"The name '{Name}' is already taken.", Name));
            }
            return new JsonResult(true);
        }
    }
}
