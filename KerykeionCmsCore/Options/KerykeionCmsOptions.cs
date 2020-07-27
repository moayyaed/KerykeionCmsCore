namespace KerykeionCmsCore.Options
{
    /// <summary>
    /// Represents all the options you can use to configure the KerykeionCms.
    /// </summary>
    public class KerykeionCmsOptions
    {
        /// <summary>
        /// Gets or sets the KerykeionCms.Options.KerykeionPageOptions for the KerykeionCms system.
        /// </summary>
        /// <value>
        /// The KerykeionCms.Options.KerykeionPageOptions for the KerykeionCms system.
        /// </value>
        public KerykeionPageOptions Pages { get; set; } = new KerykeionPageOptions();
        /// <summary>
        /// Gets or sets the KerykeionCms.Options.KerykeionUserOptions for the KerykeionCms system.
        /// </summary>
        /// <value>
        /// The KerykeionCms.Options.KerykeionUserOptions for the KerykeionCms system.
        /// </value>
        public KerykeionUserOptions User { get; set; } = new KerykeionUserOptions();
    }
}
