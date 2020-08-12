using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.PageModels;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsUI.Areas.KerykeionCms.Pages.Users
{
    [Authorize(Policy = PolicyConstants.AdministratorRequirementPolicy)]
    public class IndexModel : KerykeionPageModel
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public IndexModel(IUserService userService,
            RoleManager<IdentityRole<Guid>> roleManager,
            KerykeionTranslationsService translationsService) : base(translationsService)
        {
            _userService = userService;
            _roleManager = roleManager;
        }

        [BindProperty]
        public IndexViewModel Vm { get; set; }

        public string TxtUserName { get; set; }
        public string TxtRequiredUserName { get; set; }
        public string TxtLengthUserName { get; set; }

        public class IndexViewModel
        {
            //[DisplayName("Zoek Gebruikers")]
            //[Required(ErrorMessage = "Het is noodzakelijk om gebruikers te zoeken dat u dit veld invult.")]
            //[StringLength(50, MinimumLength = 3, ErrorMessage = "De lengte van {0} moet tussen de {2} en de {1} tekens zijn.")]
            public string SearchValue { get; set; }

            public List<UserDto> UsersFound { get; set; }
        }

        public async Task<ActionResult> OnGetAsync()
        {
            Vm = new IndexViewModel { UsersFound = new List<UserDto>() };

            PageTitle = await TranslationsService.TranslateAsync("Gebruikers");
            TxtUserName = await TranslationsService.TranslateAsync("username");
            TxtRequiredUserName = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{TxtUserName}' is required.", TxtUserName);
            TxtLengthUserName = TranslationsService.TranslateErrorByDescriber(ErrorDescriberConstants.StringLength, $"The field '{TxtUserName}' must contain a minimum of {1} and a maximum of {100} characters.", TxtUserName, 1.ToString(), 100.ToString());

            return Page();
        }

        public async Task<IActionResult> OnPostSearchUsersAsync()
        {
            if (ModelState.IsValid)
            {
                var users = await _userService.FindUsersByNameAsync(Vm.SearchValue);
                if (users.Count() > 0)
                {
                    Vm = new IndexViewModel
                    {
                        UsersFound = users.Select(u => GetUserDto(u) as UserDto).ToList()
                    };
                    return Page();
                }
            }

            ModelState.AddModelError(string.Empty, $"Er werden geen gebruikers gevonden met de zoekterm '{Vm.SearchValue}'.");
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostGetRolesAsync([FromBody] string id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            List<string> userRolesNames = await _userService.GetRolesAsync(user);

            var userRoles = new List<IdentityRole<Guid>>();
            userRolesNames.ForEach(urn => userRoles.Add(_roleManager.FindByNameAsync(urn).Result));

            return new JsonResult(new AddRoleToUserDto { Id = Guid.Parse(id), Roles = allRoles.Except(userRoles).Select(r => new RoleDto { Name = r.Name }).ToList() });
        }

        public async Task<IActionResult> OnPostAddRolesAsync()
        {
            var formDict = Request.Form.ToDictionary(k => k.Key.ToString(), k => k.Value);
            var user = await _userService.FindByIdAsync(formDict["UserId"]);
            if (user == null)
            {
                return NotFound();
            }

            var splitSelectedStringRolesNames = formDict["SelectRoles"].ToString().Split(",");
            IdentityResult result = await _userService.AddToRolesAsync(user, splitSelectedStringRolesNames);
            if (result.Succeeded)
            {
                return PageWithUser(user);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostRemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userService.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return PageWithUser(user);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userService.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return await OnGetAsync();
        }

        public override async Task<IActionResult> OnPostSetLanguageAsync()
        {
            await SetLanguageAsync();
            return await OnGetAsync();
        }

        private PageResult PageWithUser(dynamic user)
        {
            Vm = new IndexViewModel
            {
                UsersFound = new List<UserDto>
                {
                    GetUserDto(user)
                }
            };

            return Page();
        }

        private UserDto GetUserDto(dynamic user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
                ProfileImgUrl = user.ProfileImage?.Url ?? FolderConstants.DefaultUserImagePath,
                RolesNames = _userService.GetRolesAsync(user).Result
            };
        }
    }
}
