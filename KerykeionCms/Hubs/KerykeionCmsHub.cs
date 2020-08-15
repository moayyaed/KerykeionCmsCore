using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCms.Hubs
{
    public class KerykeionCmsHub : Hub
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly KerykeionImagesService _kerykeionImagesService;
        private readonly KerykeionTranslationsService _translationsService;

        public KerykeionCmsHub(RoleManager<IdentityRole<Guid>> roleManager,
            KerykeionImagesService kerykeionImagesService, 
            KerykeionTranslationsService translationsService)
        {
            _roleManager = roleManager;
            _kerykeionImagesService = kerykeionImagesService;
            _translationsService = translationsService;
        }

        #region Images
        public async Task SendSideNavImagesAsync()
        {
            await Clients.Caller.SendAsync("ReceiveSideNavImages", await _kerykeionImagesService.GetAll().OrderBy(i => i.Name).Select(i => new ImageDto
            {
                Id = i.Id,
                Name = i.Name
            }).ToListAsync());
        }

        public async Task SendMainImagesAsync()
        {
            await Clients.Caller.SendAsync("ReceiveMainImages", await _kerykeionImagesService.GetAll().OrderBy(i => i.Name).Select(i => new
            {
                i.Id,
                ImageUrl = i.Url,
                Name = $"{i.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                Created = $"{i.DateTimeCreated.Value.ToShortDateString()} - ({i.DateTimeCreated.Value.ToShortTimeString()})"
            }).ToListAsync());
        }
        #endregion

        #region Roles
        public async Task SendSideNavRolesAsync()
        {
            await Clients.Caller.SendAsync("ReceiveSideNavRoles", await ListRolesOrderedByNameAsync());
        }

        public async Task SendMainRolesAsync()
        {
            await Clients.Caller.SendAsync("ReceiveMainRoles", await ListRolesOrderedByNameAsync());
        }

        public async Task SendCreateRoleFormAsync()
        {
            var form = await _translationsService.FindDocByIdAsync(Guid.Parse("F71C6164-5395-420F-9C00-5E967CB0BE3D"));

            await Clients.Caller.SendAsync("ReceiveCreateRoleForm", form);
        }

        public async Task SendUpdateRoleFormAsync(string roleId)
        {
            var form = await _translationsService.FindDocByIdAsync(Guid.Parse("D5C8569A-2852-492D-9F64-93171B97F7FC"));

            await Clients.Caller.SendAsync("ReceiveUpdateRoleForm", form, await _roleManager.FindByIdAsync(roleId));
        }

        public async Task CreateRoleAsync(string name)
        {
            var role = new IdentityRole<Guid> { Name = name };

            await Clients.Caller.SendAsync("ReceiveCreateRoleResult", await _roleManager.CreateAsync(role), await ListRolesOrderedByNameAsync(), role);
        }

        public async Task UpdateRoleAsync(string name, string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            role.Name = name;

            await Clients.Caller.SendAsync("ReceiveUpdateRoleResult", await _roleManager.UpdateAsync(role), await ListRolesOrderedByNameAsync(), role.Name);
        }

        public async Task GetRoleAsync(string id)
        {
            await Clients.Caller.SendAsync("ReceiveRole", await _roleManager.FindByIdAsync(id));
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            await Clients.Caller.SendAsync("ReceiveDeleteRoleResult", await _roleManager.DeleteAsync(role), role);
        }

        private async Task<IEnumerable<object>> ListRolesOrderedByNameAsync()
        {
            return await _roleManager.Roles.OrderBy(r => r.Name).Select(r => new 
            {
                r.Id,
                r.Name
            }).ToListAsync();
        }
        #endregion
    }
}
