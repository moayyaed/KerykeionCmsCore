using Microsoft.AspNetCore.Authorization;

namespace KerykeionCms.PolicyRequirements
{
    public class BaseAuthorizationRequirement : IAuthorizationRequirement
    {
        public bool IsAreaRestricted { get; set; }
    }
}
