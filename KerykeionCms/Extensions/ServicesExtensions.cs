using KerykeionCms.Builders;
using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Data;
using KerykeionCmsCore.Options;
using KerykeionCmsCore.Repositories;
using KerykeionCmsCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace KerykeionCms.Extensions
{
    /// <summary>
    /// Contains extension methods for adding KerykeionCms services.
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds Kerykeion Content management services using Sqlite to the application for the specified DbContext type, including a UI, also adds Identity services.
        /// </summary>
        /// <remarks>
        /// Migrations and database still have to be made after calling this method.
        /// </remarks>
        /// <typeparam name="TContext">The type of the DbContext the KerykeionCms needs to work with.</typeparam>
        /// <param name="services">Services to configure.</param>
        /// <param name="confOpts">Configures the KerykeionCmsOptions. Optional parameter, defaults to null</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public static KerykeionCmsBuilder AddKerykeionCms<TContext>(this IServiceCollection services, Action<KerykeionCmsOptions> confOpts = null)
            where TContext : KerykeionCmsDbContext
        {
            return AddBase<KerykeionUser, TContext>(services, null, confOpts);
        }

        /// <summary>
        /// Adds Kerykeion Content management services using Sqlite to the application for the specified DbContext type, including a UI, also adds Identity services.
        /// </summary>
        /// <remarks>
        /// Migrations and database still have to be made after calling this method.
        /// </remarks>
        /// <typeparam name="TUser">The type of the User the KerykeionCms needs to work with.</typeparam>
        /// <typeparam name="TContext">The type of the DbContext the KerykeionCms needs to work with.</typeparam>
        /// <param name="services">Services to configure.</param>
        /// <param name="confOpts">Configures the KerykeionCmsOptions. Optional parameter, defaults to null</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public static KerykeionCmsBuilder AddKerykeionCms<TUser, TContext>(this IServiceCollection services, Action<KerykeionCmsOptions> confOpts = null)
            where TUser : KerykeionUser
            where TContext : KerykeionCmsDbContext<TUser>
        {
            return AddBase<TUser, TContext>(services, null, confOpts);
        }

        /// <summary>
        /// Adds Kerykeion Content management services using SqlServer to the application for the specified DbContext type, including a UI, also adds Identity services.
        /// </summary>
        /// <remarks>
        /// Migrations and database still have to be made after calling this method.
        /// </remarks>
        /// <typeparam name="TContext">The type of the DbContext the KerykeionCms needs to work with.</typeparam>
        /// <param name="services">Services to configure.</param>
        /// <param name="connectionString">The connectionstring of the Sql server database to work with.
        /// </param>
        /// <param name="confOpts">Configures the KerykeionCmsOptions. Optional parameter, defaults to null</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public static KerykeionCmsBuilder AddKerykeionCms<TContext>(this IServiceCollection services, string connectionString, Action<KerykeionCmsOptions> confOpts = null)
            where TContext : KerykeionCmsDbContext
        {
            return AddBase<KerykeionUser, TContext>(services, connectionString, confOpts);
        }

        /// <summary>
        /// Adds Kerykeion Content management services using SqlServer to the application for the specified User and DbContext types, including a UI, also adds Identity services.
        /// </summary>
        /// <remarks>
        /// Migrations and database still have to be made after calling this method.
        /// </remarks>
        /// <typeparam name="TUser">The type of the User the KerykeionCms needs to work with.</typeparam>
        /// <typeparam name="TContext">The type of the DbContext the KerykeionCms needs to work with.</typeparam>
        /// <param name="services">Services to configure.</param>
        /// <param name="connectionString">The connectionstring of the Sql server database to work with.
        /// </param>
        /// <param name="confOpts">Configures the KerykeionCmsOptions. Optional parameter, defaults to null</param>
        /// <returns>
        /// A KerykeionCmsBuilder that can be used to further configure the KerykeionCms services.
        /// </returns>
        public static KerykeionCmsBuilder AddKerykeionCms<TUser, TContext>(this IServiceCollection services, string connectionString, Action<KerykeionCmsOptions> confOpts = null)
            where TUser : KerykeionUser
            where TContext : KerykeionCmsDbContext<TUser>
        {
            return AddBase<TUser, TContext>(services, connectionString, confOpts);
        }

        private static KerykeionCmsBuilder AddBase<TUser, TContext>(this IServiceCollection services, string connectionString, Action<KerykeionCmsOptions> confOpts)
            where TUser : KerykeionUser
            where TContext : KerykeionCmsDbContext<TUser>
        {
            services.TryAddScoped<IEntitiesRepository, EntitiesRepository<TContext, TUser>>();
            services.TryAddScoped<IUserService, UserService<TUser>>();
            services.TryAddScoped<ISignInService, SignInService<TUser>>();

            services.AddDbContext<TContext>(options =>
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    options.UseSqlite($"Data Source={Directory.CreateDirectory("App_Data")}/sqlite.db");
                }
            });

            var identityBuilder = services.AddIdentity<TUser, IdentityRole<Guid>>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddClaimsPrincipalFactory<KerykeionUserClaimsPrincipalFactory<TUser>>()
                .AddErrorDescriber<KerykeionErrorDescriber>()
                .AddEntityFrameworkStores<TContext>();

            if (confOpts != null)
            {
                services.Configure(confOpts);
            }

            return new KerykeionCmsBuilder(services, identityBuilder)
                .RemoveKerykeionIdentityUIParts()
                .AddKerykeionCmsService<Webpage>()
                .AddKerykeionCmsService<Article>()
                .AddKerykeionCmsService<Link>()
                .AddKerykeionCmsService<Image>();
        }
    }
}
