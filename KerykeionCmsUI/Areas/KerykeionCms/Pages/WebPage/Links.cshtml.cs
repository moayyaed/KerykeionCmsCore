using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.WebPage
{
    [Authorize(Policy = PolicyConstants.AtLeastEditorRequirementPolicy)]
    public class LinksModel : KerykeionPageModelBase<Link>
    {
        private readonly KerykeionWebPagesService _webPagesService;

        public LinksModel(KerykeionTranslationsService translationsService,
            EntitiesService entitiesService,
            KerykeionCmsService<Link> linksService,
            KerykeionWebPagesService webPagesService) : base(translationsService, entitiesService, linksService)
        {
            _webPagesService = webPagesService;
        }

        [BindProperty]
        public LinksViewModel Vm { get; set; }


        public class LinksViewModel
        {
            public List<LinkDto> Links { get; set; } = new List<LinkDto>();
            public string Url { get; set; }
            public Guid PageId { get; set; }
        }

        public string TxtAddLink { get; set; }
        public string NameDisplay { get; set; }
        public string TxtAddedOn { get; set; }
        public string LinkNameRequiredError { get; set; }
        public string LinkNameLengthError { get; set; }
        public string LinkUrlRequiredError { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(Guid pageId)
        {
            var page = await _webPagesService.FindByIdAllIncludedAsync(pageId);
            if (page == null)
            {
                return NotFound();
            }

            PageTitle = await TranslationsService.TranslateAsync("Links");
            TxtAddLink = await TranslationsService.TranslateAsync("Add Link");
            NameDisplay = await TranslationsService.TranslateAsync("name");
            LinkNameRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{NameDisplay}' is required.", NameDisplay);
            LinkNameLengthError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{NameDisplay}' must contain a minimum of {4} and a maximum of {30} characters.", NameDisplay, 4.ToString(), 30.ToString());
            LinkUrlRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field 'Url' must contain a minimum of {4} and a maximum of {200} characters.", "Url", 4.ToString(), 200.ToString());
            TxtAddedOn = await TranslationsService.TranslateAsync("Toegevoegd op");

            ViewData["PageId"] = page.Id;
            Vm = new LinksViewModel
            {
                PageId = page.Id
            };

            if (page.Links.Count > 0)
            {
                Vm.Links = page.Links.OrderBy(l => l.UniqueNameIdentifier).Select(l => new LinkDto
                {
                    Id = l.Id,
                    Name = l?.Name,
                    Url = l?.Url,
                    DateTimeCreated = $"{l.DateTimeCreated?.ToShortDateString()} - ({l.DateTimeCreated?.ToShortTimeString()})"
                }).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var page = await _webPagesService.FindByIdAllIncludedAsync(Vm.PageId);
            var link = new Link { Url = Vm.Url, Name = Name };

            if (page == null)
            {
                return NotFound();
            }

            var addLinkResult = await Service.CreateAsync(link);
            if (addLinkResult.Successfull)
            {
                var result = await _webPagesService.AddLinkAsync(page, link);
                if (result.Successfull)
                {
                    return RedirectToPage(new { pageId = page.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            foreach (var error in addLinkResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }


            return await OnGetAsync(page.Id);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var link = await Service.FindByIdAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            var result = await Service.DeleteAsync(link);
            if (result.Successfull)
            {
                return RedirectToPage("/WebPage/Links", new { pageId = Vm.PageId });
            }

            return await OnGetAsync(Vm.PageId);
        }

        public override async Task<JsonResult> AnonymousObjectWithSortedEntitiesAsync(List<Link> sortedEntities, string pageId = null)
        {
            await Task.Delay(0);
            return new JsonResult(new
            {
                Entities = sortedEntities.Select(e => CreateDto(e)),
                TxtUpdate = BtnDetailsValue,
                TxtDelete = BtnDeleteValue,
                UpdateUrl = "WebPage/UpdateLink",
                DeleteReturnPage = "WebPage/Links",
                WebpageId = string.IsNullOrEmpty(pageId) ? "" : pageId
            });
        }

        public override object CreateDto(Link l)
        {
            return new
            {
                l.Id,
                Name = $"{l.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                Url = $"{l.Url.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{l.DateTimeCreated?.ToShortDateString()} - ({l.DateTimeCreated?.ToShortTimeString()})"
            };
        }

        public override async Task<IEnumerable<Link>> GetEntitiesToSortAsync(string pageId = null)
        {
            var page = await _webPagesService.FindByIdAllIncludedAsync(Guid.Parse(pageId));
            return page.Links;
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var pageId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["page-id"];
            return await OnGetAsync(Guid.Parse(pageId));
        }
    }
}
