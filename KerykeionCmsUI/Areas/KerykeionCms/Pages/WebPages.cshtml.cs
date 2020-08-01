using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class WebPagesModel : KerykeionPageModelBase<Webpage>
    {
        public WebPagesModel(KerykeionTranslationsService translationsService,
            KerykeionWebPagesService webPagesService) : base(translationsService, webPagesService)
        {
        }

        public string TxtAddWebpage { get; set; }
        public string TitleDisplay { get; set; }
        public string NameDisplay { get; set; }
        public string TxtAddedOn { get; set; }

        [BindProperty]
        public string Title { get; set; }

        public string PageNameRequiredError { get; set; }
        public string PageNameLengthError { get; set; }
        public string PageTitleLengthError { get; set; }

        public List<WebpagesViewModel> Pages { get; set; }

        public class WebpagesViewModel : BaseDto
        {
            public string PageName { get; set; }
            public string DateTimeCreated { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PageTitle = await TranslationsService.TranslateAsync("Webpages");
            TxtAddWebpage = await TranslationsService.TranslateAsync("Add webpage");
            TitleDisplay = await TranslationsService.TranslateAsync("title");
            NameDisplay = await TranslationsService.TranslateAsync("name");
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");

            PageNameRequiredError = TranslationsService.TranslateRequiredError(NameDisplay);
            PageNameLengthError = TranslationsService.TranslateStringLengthError(4, 30, NameDisplay);
            PageTitleLengthError = TranslationsService.TranslateStringLengthError(4, 30, TitleDisplay);

            Pages = await Service.GetAll().OrderBy(p => p.Name).Select(p => new WebpagesViewModel
            {
                Id = p.Id,
                PageName = $"{p.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{p.DateTimeCreated.Value.ToShortDateString()} - ({p.DateTimeCreated.Value.ToShortTimeString()})"
            }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            var result = await Service.CreateAsync(formDict);
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
            var page = await Service.FindByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            var result = await Service.DeleteAsync(page);
            if (result.Successfull)
            {
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Message);
            }
            return await OnGetAsync();
        }

        public async override Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<Webpage> sortedEntities, string pageId = null)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(e => CreateDto(e)),
                TxtUpdate = await TranslationsService.TranslateAsync("Details"),
                TxtDelete = await TranslationsService.TranslateAsync("Delete"),
                UpdateUrl = "WebPage",
                DeleteReturnPage = "WebPages",
                WebpageId = string.IsNullOrEmpty(pageId) ? "" : pageId
            });
        }

        public override object CreateDto(Webpage webpage)
        {
            return new
            {
                webpage.Id,
                Title = $"{webpage.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{webpage.DateTimeCreated?.ToShortDateString()} - ({webpage.DateTimeCreated?.ToShortTimeString()})"
            };
        }
    }
}
