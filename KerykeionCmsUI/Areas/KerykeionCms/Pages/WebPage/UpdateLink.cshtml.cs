using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    public class LinkModel : KerykeionPageModel
    {
        private readonly KerykeionCmsService<Link> _linksService;
        public LinkModel(KerykeionCmsService<Link> linksService,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _linksService = linksService;
        }

        [BindProperty]
        public LinkViewModel Vm { get; set; }
        public string NameDisplay { get; set; }
        public string LinkNameRequiredError { get; set; }
        public string LinkNameLengthError { get; set; }
        public string LinkUrlRequiredError { get; set; }

        public class LinkViewModel
        {
            public string Name { get; set; }
            public string Url { get; set; }

            public string LinkId { get; set; }
            public Guid PageId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var link = await _linksService.GetAll().Include(l => l.Webpage).FirstOrDefaultAsync(l => l.Id == id);
            if (link == null)
            {
                return NotFound();
            }
            ViewData["LinkId"] = link.Id;
            ViewData["PageId"] = link.Webpage.Id;

            NameDisplay = await TranslationsService.TranslateAsync("name");
            PageTitle = await TranslationsService.TranslateAsync("Update link");
            LinkNameRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{NameDisplay}' is required.", NameDisplay);
            LinkNameLengthError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{NameDisplay}' must contain a minimum of {4} and a maximum of {30} characters.", NameDisplay, 4.ToString(), 30.ToString());
            LinkUrlRequiredError = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field 'Url' must contain a minimum of {4} and a maximum of {200} characters.", "Url", 4.ToString(), 200.ToString());

            Vm = new LinkViewModel
            {
                LinkId = link.Id.ToString(),
                PageId = link.Webpage.Id,
                Name = link?.Name,
                Url = link.Url
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var link = await _linksService.FindByIdAsync(Vm.LinkId);
            if (link == null)
            {
                return NotFound();
            }

            var exists = await _linksService.ExistsAsync(Vm.Name);
            if (exists && !Vm.Name.CompleteTrimAndUpper().Equals(link.UniqueNameIdentifier))
            {
                ModelState.AddModelError(string.Empty, TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NameDuplicate, $"The name {Vm.Name} is already taken.", Vm.Name));
                return await OnGetAsync(link.Id);
            }

            link.Name = Vm?.Name;
            link.Url = Vm?.Url;

            var result = await _linksService.UpdateAsync(link);
            if (result.Successfull)
            {
                return RedirectToPage("/WebPage/UpdateLink", new { id = link.Id });
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Message);
            }

            return await OnGetAsync(link.Id);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var linkId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["link-id"];
            return await OnGetAsync(Guid.Parse(linkId));
        }
    }
}
