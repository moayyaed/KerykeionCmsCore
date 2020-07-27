namespace KerykeionCmsCore.Options
{
    /// <summary>
    /// Options for KerykeionUser validation.
    /// </summary>
    public class KerykeionUserOptions
    {
        /// <summary>
        /// Gets or sets a flag indicating whether a registering user requires a profile image.
        /// </summary>
        /// <remarks>
        /// Defaults to False.
        /// </remarks>
        /// <value>
        /// True if the application requires each user to have a profile image, otherwise False.
        /// </value>
        public bool RequireProfileImage { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether a user can make use of external services to be authenticated (like google or facebook).
        /// </summary>
        /// <remarks>
        /// Defaults to False.
        /// </remarks>
        /// <value>
        /// True if the application enables a user to authenticate via external services, False to disable.
        /// </value>
        public bool CanUseExternalAuthenticationServices { get; set; }
    }
}
