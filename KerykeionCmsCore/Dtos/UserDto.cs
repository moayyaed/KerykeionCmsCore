using System.Collections.Generic;

namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents a user dto for the KerykeionCms.
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Username for the dto.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the ProfileImgUrl for the dto.
        /// </summary>
        public string ProfileImgUrl { get; set; }
        /// <summary>
        /// Gets or sets the Email for the dto.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets a flag if the email is confirmed for the dto.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the phone number for the dto.
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets a flag if the phone number is confirmed for the dto.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the ammount of times the user has failed login in for the dto.
        /// </summary>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// Get or sets a list of role names for the dto.
        /// </summary>
        public List<string> RolesNames { get; set; }
    }
}
