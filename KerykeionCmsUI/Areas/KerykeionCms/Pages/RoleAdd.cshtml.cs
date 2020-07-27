using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Services;
using KerykeionCmsCore.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    [Authorize(Policy = PolicyConstants.AdministratorRequirementPolicy)]
    public class RoleAddModel : KerykeionPageModel
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public RoleAddModel(RoleManager<IdentityRole<Guid>> roleManager,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _roleManager = roleManager;
        }

        public string Message { get; set; }

        [BindProperty]
        public RolesViewModel Vm { get; set; }

        public class RolesViewModel
        {
            [Required(ErrorMessage = "Een naam voor een rol is verplicht.")]
            [StringLength(50, MinimumLength = 4, ErrorMessage = "De lengte van {0} moet tussen de {2} en {1} tekens zijn.")]
            [DisplayName("Rol naam")]
            public string Name { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = Vm.Name });
                if (result.Succeeded)
                {
                    return RedirectToPage();
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }
            return Page();
        }
    }
}
