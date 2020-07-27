using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace KerykeionCms.Transformers
{
    public class RouteLanguageTransformer : DynamicRouteValueTransformer
    {
        private readonly KerykeionTranslationsService _translationsService;
        public RouteLanguageTransformer(KerykeionTranslationsService translationsService)
        {
            _translationsService = translationsService;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (values["slug"] == null)
            {
                return values;
            }

            return await _translationsService.GetRouteByTextAsync(values["slug"].ToString());
        }
    }
}
