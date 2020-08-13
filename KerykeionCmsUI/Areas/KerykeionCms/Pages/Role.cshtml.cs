using KerykeionCmsCore.Constants;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages
{
    [Authorize(Policy = PolicyConstants.AdministratorRequirementPolicy)]
    public class RoleModel : KerykeionPageModel
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public RoleModel(RoleManager<IdentityRole<Guid>> roleManager,
            KerykeionTranslationsService translationsService,
            EntitiesService entitiesService) : base(translationsService, entitiesService)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public RoleViewModel Vm { get; set; }

        public class RoleViewModel
        {
            public Guid Id { get; set; }

            [Required(ErrorMessage = "Een naam voor een rol is verplicht.")]
            [StringLength(50, MinimumLength = 4, ErrorMessage = "De lengte van {0} moet tussen de {2} en {1} tekens zijn.")]
            [DisplayName("Rol naam")]
            public string Name { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            ViewData["RoleName"] = role.Name;

            Vm = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                role.Name = Vm.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage(new { id });
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

            return await OnGetAsync(id);
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            var roleId = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())["role-id"];
            return await OnGetAsync(roleId);
        }
    }
}
