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

        public KerykeionCmsHub(RoleManager<IdentityRole<Guid>> roleManager,
            KerykeionImagesService kerykeionImagesService)
        {
            _roleManager = roleManager;
            _kerykeionImagesService = kerykeionImagesService;
        }

        #region Images
        public async Task SendSideNavImagesAsync()
        {
            await Clients.Caller.SendAsync("GetSideNavImages", await _kerykeionImagesService.GetAll().OrderBy(i => i.Name).Select(i => new ImageDto
            {
                Id = i.Id,
                Name = i.Name
            }).ToListAsync());
        }

        public async Task SendMainImagesAsync()
        {
            await Clients.Caller.SendAsync("GetMainImages", await _kerykeionImagesService.GetAll().OrderBy(i => i.Name).Select(i => new ImageDto
            {
                Id = i.Id,
                Url = i.Url,
                Name = $"{i.Name.SubstringMaxLengthOrGivenLength(0, 20)}",
                DateTimeCreated = $"{i.DateTimeCreated.Value.ToShortDateString()} - ({i.DateTimeCreated.Value.ToShortTimeString()})"
            }).ToListAsync());
        }
        #endregion

        #region Roles
        public async Task SendSideNavRolesAsync()
        {
            await Clients.Caller.SendAsync("GetSideNavRoles", await ListRolesOrderedByNameAsync());
        }

        public async Task SendMainRolesAsync()
        {
            await Clients.Caller.SendAsync("GetMainRoles", await ListRolesOrderedByNameAsync());
        }

        private async Task<IEnumerable<IdentityRole<Guid>>> ListRolesOrderedByNameAsync()
        {
            return await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }
        #endregion
    }
}
