using KerykeionCms.Transformers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace KerykeionCms.Extensions
{
    /// <summary>
    /// Contains extension methods for adding necessary middlewares for the KerykeionCms service.
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Adds necessary middlewares for the KerykeionCms service to the calling application.
        /// </summary>
        /// <param name="app">The calling application.</param>
        /// <returns>
        /// A reference to this instance after the operation has completed.
        /// </returns>
        public static IApplicationBuilder UseKerykeionCms(this IApplicationBuilder app)
        {
            return app.UseKerykeionCms(o =>
            {
                o.MapRazorPages();
                o.MapDynamicPageRoute<RouteLanguageTransformer>("/{**slug}");
            });
        }

        /// <summary>
        /// Adds necessary middlewares for the KerykeionCms service to the calling application.
        /// </summary>
        /// <param name="app">The calling application.</param>
        /// <param name="endpoints">The endpoints to configure.</param>
        /// <returns>
        /// A reference to this instance after the operation has completed.
        /// </returns>
        public static IApplicationBuilder UseKerykeionCms(this IApplicationBuilder app, Action<IEndpointRouteBuilder> endpoints)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints);
            return app;
        }
    }
}
