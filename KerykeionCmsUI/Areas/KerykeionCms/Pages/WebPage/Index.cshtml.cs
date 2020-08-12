using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.WebPage
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class IndexModel : KerykeionPageModel
    {
        private readonly KerykeionWebPagesService _webpagesService;
        public IndexModel(KerykeionWebPagesService webpagesService,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _webpagesService = webpagesService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public string TitleDisplay { get; set; }
        public string NameDisplay { get; set; }
        public string TxtSeeArticles { get; set; }
        public string TxtSeeLinks { get; set; }
        public string PageTitleLengthError { get; set; }

        [BindProperty]
        public WebPageViewModel Vm { get; set; }

        public class WebPageViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var page = await _webpagesService.FindByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            TitleDisplay = await TranslationsService.TranslateAsync("title");
            NameDisplay = await TranslationsService.TranslateAsync("name");
            TxtSeeArticles = await TranslationsService.TranslateAsync("Bekijk artikels");
            TxtSeeLinks = await TranslationsService.TranslateAsync("Bekijk links");
            PageTitle = await TranslationsService.TranslateAsync("Webpagina bijwerken");
            PageTitleLengthError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{TitleDisplay}' must contain a minimum of {4} and a maximum of {30} characters.", TitleDisplay, 4.ToString(), 30.ToString());
            ViewData["PageId"] = page.Id;

            Vm = new WebPageViewModel
            {
                Id = page.Id,
                Name = page.Name,
                Title = page?.Title
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(string id)
        {
            var page = await _webpagesService.FindByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                page.Title = Vm?.Title;
                var result = await _webpagesService.UpdateAsync(page);
                if (result.Successfull)
                {
                    return RedirectToPage(new { id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            return await OnGetAsync(id);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var page = await _webpagesService.FindByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            var result = await _webpagesService.DeleteAsync(page);
            if (result.Successfull)
            {
                StatusMessage = "The webpage has been successfully deleted.";
                return RedirectToPage("/WebPages");
            }

            foreach (var error in result.Errors)
            {
                StatusMessage += $"Error: {error.Message}<br />";
            }
            return await OnGetAsync(id);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var pageId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["page-id"];
            return await OnGetAsync(pageId);
        }
    }
}
