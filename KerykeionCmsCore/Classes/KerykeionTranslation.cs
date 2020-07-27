using KerykeionCmsCore.Enums;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Routing;

namespace KerykeionCmsCore.Classes
{
    /// <summary>
    /// Represents a translation in the KerykeionCms.
    /// </summary>
    public class KerykeionTranslation : KerykeionBaseClass
    {
        /// <summary>
        /// Gets or sets the English translation.
        /// </summary>
        public string English { get; set; }
        /// <summary>
        /// Gets or sets the French translation.
        /// </summary>
        public string French { get; set; }
        /// <summary>
        /// Gets or sets the German translation.
        /// </summary>
        public string German { get; set; }
        /// <summary>
        /// Gets or sets the Dutch translation.
        /// </summary>
        public string Dutch { get; set; }
        /// <summary>
        /// Gets all translations delimited by a semicolumn.
        /// </summary>
        public string AllTranslationsDelimitedBySemiColumn => $"{English};{French};{German};{Dutch}".CompleteTrimAndUpper();
        /// <summary>
        /// Gets or sets the translation's error describer to identify the translation via the error.
        /// </summary>
        public string ErrorDescriber { get; set; }
        /// <summary>
        /// Gets or sets the route for the given language.
        /// </summary>
        public RouteValueDictionary Route => ConstructRoute();
        private RouteValueDictionary ConstructRoute()
        {
            if (string.IsNullOrEmpty(AreaRoute))
            {
                return new RouteValueDictionary()
                {
                    {"page", PageRoute ?? ""},
                };
            }

            return new RouteValueDictionary()
            {
                {"page", PageRoute ?? ""},
                {"area", AreaRoute ?? ""}
            };
        }
        /// <summary>
        /// Gets or sets the razor page area for the translated route.
        /// </summary>
        public string AreaRoute { get; set; }
        /// <summary>
        /// Gets or sets the razor page page for the translated route.
        /// </summary>
        public string PageRoute { get; set; }
        /// <summary>
        /// Translates a string to the specified language.
        /// </summary>
        /// <param name="language">The language to be translated to.</param>
        /// <returns>
        /// Returns a to the specified language translated string.
        /// </returns>
        public string Translate(KerykeionCmsLanguage language)
        {
            if (language == KerykeionCmsLanguage.NL) return Dutch;
            if (language == KerykeionCmsLanguage.FR) return French;
            if (language == KerykeionCmsLanguage.DE) return German;
            return English;
        }
    }
}
