using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace KerykeionCmsCore.Data
{
    /// <summary>
    /// Base class for the Entity framework database context used for the KerykeionCms.
    /// </summary>
    /// <typeparam name="TUser">The type of user objects to context needs to work with.</typeparam>
    public class KerykeionCmsDbContext<TUser> : IdentityDbContext<TUser, IdentityRole<Guid>, Guid>
        where TUser : KerykeionUser
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="options">The options to be used by a Microsoft.EntityFrameworkCore.DbContext.</param>
        public KerykeionCmsDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DB Images.
        /// </summary>
        public DbSet<Image> Images { get; set; }
        /// <summary>
        /// Gets or sets the DB Webpages.
        /// </summary>
        public DbSet<Webpage> Webpages { get; set; }
        /// <summary>
        /// Gets or sets the DB Articles.
        /// </summary>
        public DbSet<Article> Articles { get; set; }
        /// <summary>
        /// Gets or sets the DB Links.
        /// </summary>
        public DbSet<Link> Links { get; set; }

        /// <summary>
        /// Configures the schema needed for the KerykeionCms.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ILookupNormalizer normalizer = new UpperInvariantLookupNormalizer();

            builder.Entity<IdentityRole<Guid>>().HasData
                (
                 new IdentityRole<Guid> { Name = RoleContstants.Administrator, Id = new Guid("A2EB5341-22E7-43C7-AC0E-C4AFED51DB2B"), NormalizedName = normalizer.NormalizeName(RoleContstants.Administrator) },
                 new IdentityRole<Guid> { Name = RoleContstants.Editor, Id = new Guid("57F5DC72-FA6D-4038-B337-D00BEF5A759A"), NormalizedName = normalizer.NormalizeName(RoleContstants.Editor) },
                 new IdentityRole<Guid> { Name = RoleContstants.RegularUser, Id = new Guid("2DD7B94B-CE9A-473A-B955-2FAD487BD435"), NormalizedName = normalizer.NormalizeName(RoleContstants.RegularUser) }
                );
        }
    }

    /// <summary>
    /// Base class for the Entity framework database context used for the KerykeionCms.
    /// </summary>
    public class KerykeionCmsDbContext : KerykeionCmsDbContext<KerykeionUser>
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="options">The options to be used by a Microsoft.EntityFrameworkCore.DbContext.</param>
        public KerykeionCmsDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
