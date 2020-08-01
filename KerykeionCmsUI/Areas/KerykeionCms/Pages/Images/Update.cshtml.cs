using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Images
{
    public class UpdateModel : KerykeionPageModel
    {
        private readonly KerykeionImagesService _imagesService;
        public UpdateModel(KerykeionImagesService imagesService,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _imagesService = imagesService;
        }

        public string TitleDisplay { get; set; }
        public string ImageDisplay { get; set; }
        public string TitleRequiredError { get; set; }
        public string TitleLengthError { get; set; }
        public List<ForeignKeyDto> ForeignKeyProperties { get; set; }

        [BindProperty]
        public string ImageId { get; set; }

        [BindProperty]
        public UpdateImageViewModel Vm { get; set; }

        public class UpdateImageViewModel
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public IFormFile File { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var image = await _imagesService.FindByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            PageTitle = await TranslationsService.TranslateAsync("images");
            TitleDisplay = await TranslationsService.TranslateAsync("title");
            ImageDisplay = await TranslationsService.TranslateAsync("image");
            TitleRequiredError = TranslationsService.TranslateRequiredError(TitleDisplay);
            TitleLengthError = TranslationsService.TranslateStringLengthError(4, 30, TitleDisplay);

            ForeignKeyProperties = _imagesService.GetForeignKeyPropertiesToDto(image).ToList();

            ViewData["ImgId"] = id;
            ImageId = id;

            Vm = new UpdateImageViewModel
            {
                Title = image.Name,
                Url = image.Url
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var image = await _imagesService.FindByIdAsync(ImageId);
            if (image == null)
            {
                return NotFound();
            }

            var formForeignKeys = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value).Where(k => k.Key.Contains("FOREIGNKEY", StringComparison.OrdinalIgnoreCase));

            if (!Vm.Title.Equals(image.Name))
            {
                image.Name = Vm.Title;
            }

            if (Vm.File == null)
            {
                var result = await _imagesService.UpdateAsync(image, formForeignKeys);
                if (result.Successfull)
                {
                    return RedirectToPage(new { id = image.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }

                return await OnGetAsync(image.Id.ToString());
            }

            var updateResult = await _imagesService.UpdateAsync(image, Vm.File, formForeignKeys);
            if (updateResult.Successfull)
            {
                return RedirectToPage(new { id = image.Id });
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(image.Id.ToString());
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var image = await _imagesService.FindByIdAsync(ImageId);
            if (image == null)
            {
                return NotFound();
            }

            var result = await _imagesService.RemoveImageAndDeleteAsync(image);
            if (result.Successfull)
            {
                return RedirectToPage("/Images/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync(image.Id.ToString());
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString());
            return await OnGetAsync(formDict["image-id"].ToString());
        }
    }
}
