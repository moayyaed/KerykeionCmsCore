using KerykeionCmsCore.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace KerykeionCms.PolicyRequirements
{
    class AtLeastEditorRoleRequirementHandler : AuthorizationHandler<AtLeastEditorRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AtLeastEditorRoleRequirement requirement)
        {
            if (requirement.IsAreaRestricted)
            {
                if (context.User.IsInRole(RoleContstants.Administrator) || context.User.IsInRole(RoleContstants.Editor))
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
