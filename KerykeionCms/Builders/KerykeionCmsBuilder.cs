using ImageManagement.Services;
using KerykeionCms.Options;
using KerykeionCms.PolicyRequirements;
using KerykeionCms.Transformers;
using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Options;
using KerykeionCmsCore.Services;
using KerykeionIdentityUI.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace KerykeionCms.Builders
{
    /// <summary>
    /// The KerykeionCmsBuilder that can be used to configure the KerykeionCms services.
    /// </summary>
    public class KerykeionCmsBuilder
    {
        private IServiceCollection Services { get; set; }
        private IdentityBuilder IdentityBuilder { get; set; }


        public KerykeionCmsBuilder(IServiceCollection services, IdentityBuilder identityBuilder)
        {
            Services = services;
            IdentityBuilder = identityBuilder;

            Services.TryAddScoped<EntitiesService>();
            Services.TryAddScoped<ImagesService>();
            Services.TryAddScoped<KerykeionImagesService>();
            Services.TryAddScoped<KerykeionWebPagesService>();
            Services.TryAddScoped<KerykeionArticlesService>();
            Services.TryAddScoped<KerykeionTranslationsService>();
            Services.TryAddScoped<RouteLanguageTransformer>();
            Services.TryAddScoped<ThemesService>();
            Services.TryAddTransient<IEmailService, EmailService>();


            var kerykeionOpts = Services.BuildServiceProvider().GetService<IOptions<KerykeionCmsOptions>>().Value;

            Services.AddAuthorization(options => options.AddPolicy(PolicyConstants.AdministratorRequirementPolicy,
                policy => policy.AddRequirements(new AdministratorRoleRequirement(kerykeionOpts.Pages.RequireAdministratorRoleToAccessCmsPages))));

            Services.AddAuthorization(options => options.AddPolicy(PolicyConstants.AtLeastEditorRequirementPolicy,
                    policy => policy.AddRequirements(new AtLeastEditorRoleRequirement(kerykeionOpts.Pages.RequireAdministratorRoleToAccessCmsPages))));

            Services.ConfigureOptions(typeof(KerykeionCmsConfigurationOptions));

            Services.AddSingleton<IAuthorizationHandler, AdministratorRoleRequirementHandler>();
            Services.AddSingleton<IAuthorizationHandler, AtLeastEditorRoleRequirementHandler>();

            Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/";
                options.SlidingExpiration = true;
            });

            Services.AddRazorPages();
            Services.AddSignalR();
        }

        /// <summary>
        /// Call this function to add a service for the TEntity type to the service collection.
        /// </summary>
        /// <typeparam name="TEntity">The entity type you want to use the KerykeionCmsService for.</typeparam>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public virtual KerykeionCmsBuilder AddKerykeionCmsService<TEntity>()
            where TEntity : KerykeionBaseClass
        {
            Services.TryAddScoped<KerykeionCmsService<TEntity>>();
            return this;
        }

        /// <summary>
        /// Call this method to configure an smtp email sender for confirming email and retrieving password purposes.
        /// </summary>
        /// <param name="websiteName">Your application name (or any name you want to use as a sender).</param>
        /// <param name="websiteEmailAddress">The email address to use as a sender.</param>
        /// <param name="smtpHostName">The smtp host name.</param>
        /// <param name="smtpHostPort">The smtp host port.</param>
        /// <param name="smtpUseSsl">Set this true if you want to use ssl (https).</param>
        /// <param name="authenticatedEmailAddress">The authenticated email address or username to be used.</param>
        /// <param name="authenticatedEmailAddressPassword">The authenticated email address password.</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public virtual KerykeionCmsBuilder ConfigureSmtpEmailService(string websiteName, string websiteEmailAddress, string smtpHostName, int smtpHostPort, bool smtpUseSsl, string authenticatedEmailAddress, string authenticatedEmailAddressPassword)
        {
            Services.Configure<SendSmtpEmailOptions>(options =>
            {
                options.WebsiteName = websiteName;
                options.WebsiteEmailAddress = websiteEmailAddress;
                options.SmtpHostName = smtpHostName;
                options.SmtpHostPort = smtpHostPort;
                options.SmtpUseSsl = smtpUseSsl;
                options.AuthenticatedEmailAddress = authenticatedEmailAddress;
                options.AuthenticatedEmailAddressPassword = authenticatedEmailAddressPassword;
                options.IsConfigured = true;
            });
            return this;
        }

        /// <summary>
        /// Call this method to configure the identity options.
        /// </summary>
        /// <param name="options">The identity options to configure.</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public virtual KerykeionCmsBuilder ConfigureIdentityOptions(Action<IdentityOptions> options)
        {
            if (options != null)
            {
                Services.Configure(options);
            }
            return this;
        }

        /// <summary>
        /// Adds a Kerykeion built default Identity pages UI.
        /// </summary>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public virtual KerykeionCmsBuilder AddKerykeionIdentityUI()
        {
            var kerykeionCmsUIAssembly = typeof(RegisterModel).Assembly;
            var kerykeionIdentityUIrelatedAssembly = RelatedAssemblyAttribute.GetRelatedAssemblies(kerykeionCmsUIAssembly, throwOnError: true).FirstOrDefault(a => a.FullName.Contains("KerykeionIdentityUI.Views", StringComparison.OrdinalIgnoreCase));
            Services.AddMvc()
                .ConfigureApplicationPartManager(apm => 
                {
                    apm.ApplicationParts.Remove(apm.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionLoginUI", StringComparison.OrdinalIgnoreCase)));
                    apm.ApplicationParts.Remove(apm.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionLoginUI.Views", StringComparison.OrdinalIgnoreCase)));
                    apm.ApplicationParts.Remove(apm.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionLoginUI.Views", StringComparison.OrdinalIgnoreCase)));
                }).AddApplicationPart(kerykeionIdentityUIrelatedAssembly);

            IdentityBuilder.AddDefaultUI();

            Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            return this;
        }

        internal KerykeionCmsBuilder RemoveKerykeionIdentityUIParts()
        {
            Services.AddMvc()
                .ConfigureApplicationPartManager(partManager =>
                {
                    partManager.ApplicationParts.Remove(partManager.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionIdentityUI", StringComparison.OrdinalIgnoreCase)));
                    partManager.ApplicationParts.Remove(partManager.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionIdentityUI.Views", StringComparison.OrdinalIgnoreCase)));
                    partManager.ApplicationParts.Remove(partManager.ApplicationParts.FirstOrDefault(p => p.Name.Equals("KerykeionIdentityUI.Views", StringComparison.OrdinalIgnoreCase)));
                });
            return this;
        }
    }
}
