namespace KerykeionCms.PolicyRequirements
{
    public class AtLeastEditorRoleRequirement : BaseAuthorizationRequirement
    {
        public AtLeastEditorRoleRequirement(bool isAreaRestricted)
        {
            IsAreaRestricted = isAreaRestricted;
        }
    }
}
