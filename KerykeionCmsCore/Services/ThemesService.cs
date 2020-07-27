using KerykeionCmsCore.Options;
using Microsoft.Extensions.Options;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ThemesService
    {
        private readonly KerykeionCmsOptions _options;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ThemesService(IOptions<KerykeionCmsOptions> options)
        {
            _options = options.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        public string BgAndColorTheme => GetBgAndColorClasses();
        /// <summary>
        /// 
        /// </summary>
        public string OpenSideNavTheme => GetOpenSideNavTheme();

        private string GetOpenSideNavTheme()
        {
            return _options.Pages.Theme switch
            {
                Enums.KerykeionCmsTheme.Light => "bg-black text-white",
                Enums.KerykeionCmsTheme.Dark => "bg-light text-dark",
                _ => "bg-black text-white",
            };
        }

        private string GetBgAndColorClasses()
        {
            return _options.Pages.Theme switch
            {
                Enums.KerykeionCmsTheme.Dark => "bg-black text-white",
                Enums.KerykeionCmsTheme.Light => "bg-light text-dark",
                _ => "bg-black text-white",
            };
        }
    }
}
