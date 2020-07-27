using KerykeionCmsCore.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents a user in the KerykeionCms.
    /// </summary>
    public class KerykeionUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the profile image for the user.
        /// </summary>
        public Image ProfileImage { get; set; }
        /// <summary>
        /// Gets or sets the language for the user.
        /// </summary>
        public KerykeionCmsLanguage Language { get; set; }
    }
}
