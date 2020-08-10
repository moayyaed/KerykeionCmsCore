using System;
using System.Collections.Generic;
using System.Linq;

namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents a Webpage in the KerykeionCms.
    /// </summary>
    /// <remarks>
    /// Can be linked to a specific webpage in the calling application.
    /// </remarks>
    public class Webpage : KerykeionBaseClass
    {
        /// <summary>
        /// Gets or sets the title for the webpage.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets a collection of links for the webpage.
        /// </summary>
        public ICollection<Link> Links { get; set; }
        /// <summary>
        /// Gets or sets a collection of articles for the webpage.
        /// </summary>
        public ICollection<Article> Articles { get; set; }

        /// <summary>
        /// Searches the webpage article by the specified article ID.
        /// </summary>
        /// <param name="articleId">The article ID to search for.</param>
        /// <returns>
        /// A Webpage article which matches the specified ID.
        /// </returns>
        public Article FindArticleById(string articleId)
        {
            if (!Guid.TryParse(articleId, out _))
            {
                return null;
            }
            return Articles.FirstOrDefault(a => a.Id == Guid.Parse(articleId));
        }

        /// <summary>
        /// Searches the webpage article by the specified article Name.
        /// </summary>
        /// <param name="name">The article name to search for.</param>
        /// <returns>
        /// A Webpage article which matches the specified ID.
        /// </returns>
        public Article FindArticleByName(string name)
        {
            return Articles.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the webpage link by the specified link ID.
        /// </summary>
        /// <param name="linkId">The link ID to search for.</param>
        /// <returns>
        /// A webpage link which matches the specified ID.
        /// </returns>
        public Link FindLinkById(string linkId)
        {
            return Links.FirstOrDefault(l => l.Id == Guid.Parse(linkId));
        }
    }
}
