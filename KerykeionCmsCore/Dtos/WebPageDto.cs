using System.Collections.Generic;

namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents a webpage dto for the kerykeionCms.
    /// </summary>
    public class WebPageDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Name for the dto.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Articles for the dto.
        /// </summary>
        public List<ArticleDto> Articles { get; set; }
        /// <summary>
        /// Gets or sets the links for the dto.
        /// </summary>
        public List<LinkDto> Links { get; set; }
    }
}
