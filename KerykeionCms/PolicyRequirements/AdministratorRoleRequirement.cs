namespace KerykeionCms.PolicyRequirements
{
    public class AdministratorRoleRequirement : BaseAuthorizationRequirement
    {
        public AdministratorRoleRequirement(bool isAreaRestricted)
        {
            IsAreaRestricted = isAreaRestricted;
        }
    }
}
