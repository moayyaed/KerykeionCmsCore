using KerykeionCmsCore.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace KerykeionCms.PolicyRequirements
{
    public class AdministratorRoleRequirementHandler : AuthorizationHandler<AdministratorRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorRoleRequirement requirement)
        {
            if (requirement.IsAreaRestricted)
            {
                if (context.User.IsInRole(RoleContstants.Administrator))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
