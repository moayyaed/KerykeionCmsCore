using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
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
    public class CreateModel : KerykeionPageModelBase<Image>
    {
        private readonly KerykeionImagesService _imagesService;

        public CreateModel(KerykeionImagesService imagesService,
            KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionCmsService<Image> service) : base(translationsService, entitiesService, service)
        {
            _imagesService = imagesService;
        }


        public string TitleDisplay { get; set; }
        public string ImageDisplay { get; set; }
        public string TitleRequiredError { get; set; }
        public string TitleLengthError { get; set; }
        public string FileRequiredError { get; set; }
        public List<string> ForeignKeyPropertyNames { get; set; }

        [BindProperty]
        public CreateImageViewModel Vm { get; set; }

        public class CreateImageViewModel
        {
            public string Title { get; set; }
            public IFormFile File { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ForeignKeyPropertyNames = _imagesService.GetForeignKeyProperties()
                                                .Select(fk => fk.Name).ToList();

            PageTitle = await TranslationsService.TranslateAsync("Create Image");
            TitleDisplay = await TranslationsService.TranslateAsync("title");
            ImageDisplay = await TranslationsService.TranslateAsync("image");
            TitleRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{TitleDisplay}' is required.", TitleDisplay);
            TitleLengthError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{TitleDisplay}' must contain a minimum of {2} and a maximum of {30} characters.", TitleDisplay, 2.ToString(), 30.ToString());
            FileRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{ImageDisplay}' is required.", ImageDisplay);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var image = new Image { Name = Vm.Title };
            var formForeignKeys = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value).Where(k => k.Key.Contains("FOREIGNKEY", StringComparison.OrdinalIgnoreCase));

            var result = await _imagesService.CreateAsync(image, Vm.File, formForeignKeys);
            if (result.Successfull)
            {
                return RedirectToPage("/Images/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return await OnGetAsync();
        }
    }
}
