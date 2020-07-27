namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents a Link in the KerykeionCms.
    /// </summary>
    public class LinkDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Url for the dto.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the Name for the dto.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the DateTime Created for the dto.
        /// </summary>
        public string DateTimeCreated { get; set; }
    }
}
