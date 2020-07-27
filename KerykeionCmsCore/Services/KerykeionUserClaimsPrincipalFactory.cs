using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    public class KerykeionUserClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser, IdentityRole<Guid>>
        where TUser : KerykeionUser
    {
        public KerykeionUserClaimsPrincipalFactory(UserManager<TUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Language", user.Language.ToString() ?? KerykeionCmsLanguage.EN.ToString()));
            return identity;
        }

        public override Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            return base.CreateAsync(user);
        }
    }
}
