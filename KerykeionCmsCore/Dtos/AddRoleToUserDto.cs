using System.Collections.Generic;

namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents an AddRoleToUser dto in the KerykeionCms.
    /// </summary>
    public class AddRoleToUserDto : BaseDto
    {
        /// <summary>
        /// Gets or sets a list of RoleDto for the dto.
        /// </summary>
        public List<RoleDto> Roles { get; set; }
    }
}
