namespace KerykeionCmsCore.Dtos
{
    /// <summary>
    /// Represents a PickedLanguage dto in the KerykeionCms.
    /// </summary>
    public class PickedLanguageDto
    {
        /// <summary>
        /// Gets or sets the ISO format of the dto language.
        /// </summary>
        public string ShortLanguage { get; set; }
        /// <summary>
        /// Gets or sets the full format of the dto language.
        /// </summary>
        public string LongLanguage { get; set; }
    }
}
