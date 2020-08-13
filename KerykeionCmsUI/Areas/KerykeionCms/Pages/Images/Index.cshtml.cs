using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Images
{
    public class IndexModel : KerykeionPageModelBase<Image>
    {
        private readonly KerykeionImagesService _imagesService;
        public IndexModel(KerykeionImagesService imagesService,
            KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionCmsService<Image> service) : base(translationsService, entitiesService, service)
        {
            _imagesService = imagesService;
        }

        public string TitleDisplay { get; set; }
        public string TxtAddedOn { get; set; }
        public List<ImageDto> Images { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var images = await _imagesService.ListAllAsync();
            if (images == null)
            {
                return NotFound();
            }

            PageTitle = await TranslationsService.TranslateAsync("images");
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");
            TitleDisplay = await TranslationsService.TranslateAsync("title");

            Images = images.OrderBy(i => i.Name).Select(i => new ImageDto
            {
                Id = i.Id,
                Url = i.Url,
                Name = $"{i.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{i.DateTimeCreated.Value.ToShortDateString()} - ({i.DateTimeCreated.Value.ToShortTimeString()})"
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var image = await _imagesService.FindByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var result = await _imagesService.RemoveImageAndDeleteAsync(image);
            if (result.Successfull)
            {
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync();
        }

        public override async Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<Image> sortedEntities, string pageId = null)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(e => CreateDto(e)),
                TxtUpdate = BtnDetailsValue,
                TxtDelete = BtnDeleteValue,
                UpdateUrl = "Images/Update",
                DeleteReturnPage = "Images/Index",
                WebpageId = string.IsNullOrEmpty(pageId) ? "" : pageId
            });
        }

        public override object CreateDto(Image i)
        {
            return new
            {
                i.Id,
                i.Url,
                Name = $"{i.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{i.DateTimeCreated?.ToShortDateString()} - ({i.DateTimeCreated?.ToShortTimeString()})"
            };
        }
    }
}
