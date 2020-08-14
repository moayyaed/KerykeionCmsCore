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

        public KerykeionCmsHub(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

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
    }
}
