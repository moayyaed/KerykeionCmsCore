namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents a URL in the KerykeionCms.
    /// </summary>
    public class Link : KerykeionBaseClass
    {
        /// <summary>
        /// Gets or sets the universal resource link.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the Webpage this link is assigned to.
        /// </summary>
        public Webpage Webpage { get; set; }
    }
}
