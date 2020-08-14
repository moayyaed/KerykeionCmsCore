using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task SendRolesAsync()
        {
            await Clients.Caller.SendAsync("GetRoles", await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync());
        }
    }
}
