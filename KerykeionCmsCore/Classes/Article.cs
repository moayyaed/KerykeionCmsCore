using System.Collections.Generic;

namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents an Article in the KerykieonCms.
    /// </summary>
    /// <remarks>
    /// <para>Can also be used to add a description to a certain Entity.</para>
    /// <para>Contains a collection of Images.</para>
    /// </remarks>
    public class Article : KerykeionBaseClass
    {
        /// <summary>
        /// Gets or sets the MarkdownText for the Article.
        /// </summary>
        public string MarkdownText { get; set; }
        /// <summary>
        /// Gets or sets a Webpage attached to an instance of an Article.
        /// </summary>
        public Webpage Webpage { get; set; }
        /// <summary>
        /// Gets or sets a collection of images attached to an instance of an Article.
        /// </summary>
        public ICollection<Image> Images { get; set; }

    }
}
