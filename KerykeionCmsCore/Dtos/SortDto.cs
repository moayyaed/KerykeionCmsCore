namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents a sorting dto for the KerykeionCms.
    /// </summary>
    public class SortDto
    {
        /// <summary>
        /// Gets or sets the sorting order for the dto.
        /// </summary>
        public string SortingOrder { get; set; }
        /// <summary>
        /// Gets or sets the PageId (Webpage) for the dto.
        /// </summary>
        public string PageId { get; set; }
        /// <summary>
        /// Gets or sets the Table (DB Table) for the dto.
        /// </summary>
        public string Table { get; set; }
    }
}
