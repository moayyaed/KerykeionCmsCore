using KerykeionCmsCore.Classes;
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
            KerykeionCmsService<Image> service) : base(translationsService, service)
        {
            _imagesService = imagesService;
        }

        public string TitleDisplay { get; set; }
        public string ImageDisplay { get; set; }
        public string TitleRequiredError { get; set; }
        public string TitleLengthError { get; set; }
        public string FileRequiredError { get; set; }
        public string TxtAddImage { get; set; }
        public string TxtAddedOn { get; set; }
        public List<ImageDto> Images { get; set; }

        [BindProperty]
        public AddImageViewModel Vm { get; set; }
        public List<string> ForeignKeyPropertyNames { get; set; }


        public class AddImageViewModel
        {
            public string Title { get; set; }
            public IFormFile File { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var images = await _imagesService.ListAllAsync();
            if (images == null)
            {
                return NotFound();
            }

            PageTitle = await TranslationsService.TranslateAsync("images");
            TitleDisplay = await TranslationsService.TranslateAsync("title");
            ImageDisplay = await TranslationsService.TranslateAsync("image");
            TxtAddImage = await TranslationsService.TranslateAsync("add an image");
            TitleRequiredError = TranslationsService.TranslateRequiredError(TitleDisplay);
            TitleLengthError = TranslationsService.TranslateStringLengthError(2, 30, TitleDisplay);
            FileRequiredError = TranslationsService.TranslateRequiredError(ImageDisplay);
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");

            ForeignKeyPropertyNames = _imagesService.GetForeignKeyProperties()
                                                .Select(fk => fk.Name).ToList();

            Images = images.OrderBy(i => i.Name).Select(i => new ImageDto
            {
                Id = i.Id,
                Url = i.Url,
                Name = $"{i.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{i.DateTimeCreated.Value.ToShortDateString()} - ({i.DateTimeCreated.Value.ToShortTimeString()})"
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var image = new Image { Name = Vm.Title };
            var formForeignKeys = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value).Where(k => k.Key.Contains("FOREIGNKEY", StringComparison.OrdinalIgnoreCase));

            var result = await _imagesService.CreateAsync(image, Vm.File, formForeignKeys);
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
