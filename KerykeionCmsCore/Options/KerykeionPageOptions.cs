using KerykeionCmsCore.Enums;

namespace KerykeionCmsCore.Options
{
    /// <summary>
    /// Provides options for the KerykeionCms pages.
    /// </summary>
    public class KerykeionPageOptions
    {
        /// <summary>
        /// Gets or sets a flag indicating whether it is possible for a user of the application to register.
        /// </summary>
        /// <value>
        /// Defaults to true. Set to false to disable users registering into the application.
        /// </value>
        public bool IsRegisteringEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a flag indicating whether it is possible for a user of the application to log in.
        /// </summary>
        /// <value>
        /// Defaults to true. Set to false to disable users loginin into the application.
        /// </value>
        public bool IsLoginInEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a flag indicating whether it is required to be in an Administrator role in order to access Content Management Pages.
        /// </summary>
        /// <remarks>
        /// Should only be set to false in development environments.
        /// </remarks>
        /// <value>
        /// Defaults to true. Set to false to enable anyone to access Content Management pages.
        /// </value>
        public bool RequireAdministratorRoleToAccessCmsPages { get; set; } = true;

        /// <summary>
        /// Gets or sets the theme for the KerykeionCms pages.
        /// </summary>
        /// <value>
        /// Defaults to Dark theme. There is also a Light theme.
        /// </value>
        public KerykeionCmsTheme Theme { get; set; } = KerykeionCmsTheme.Dark;

        /// <summary>
        /// Gets or sets the language for the pages.
        /// </summary>
        /// <value>
        /// Defaults to English but there is also Dutch, German and French to choose from.
        /// </value>
        public KerykeionCmsLanguage Language { get; set; } = KerykeionCmsLanguage.EN;
    }
}
