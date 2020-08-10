using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for delivering CRUD operations on webpages.
    /// </summary>
    public class KerykeionWebPagesService : KerykeionCmsService<Webpage>
    {
        /// <summary>
        /// Constructs a new instance of KerykeionCms.Services.KerykeionWebPagesService
        /// </summary>
        /// <param name="service">The database service the KerykeionWebPagesService will operate over.</param>
        public KerykeionWebPagesService(EntitiesService service) : base(service)
        {
        }

        /// <summary>
        /// Gets all the webpages in the database to query.
        /// </summary>
        public IQueryable<Webpage> Pages => GetAll();

        /// <summary>
        /// Searches for the webpage by the specified ID.
        /// </summary>
        /// <param name="id">The ID to search for.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing a webpage with the specified ID.
        /// </returns>
        public async Task<Webpage> FindByIdAllIncludedAsync(Guid? id)
        {
            var pages = await ListAllIncludedAsync();
                return pages.FirstOrDefault(p => p.Id.Equals(id));
        }

        /// <summary>
        /// Searches for the webpage by the specified name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing a webpage with the specified name.
        /// </returns>
        public async Task<Webpage> FindByNameAllIncludedAsync(string name)
        {
            var pages = await ListAllIncludedAsync();
                return pages.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<KerykeionDbResult> AddArticleAsync(Webpage page, Article article)
        {
            if (page == null || article == null)
            {
                return KerykeionDbResult.Fail();
            }

            var p = await FindByIdAllIncludedAsync(page.Id);
            if (p == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The page to add articles returned null!" });
            }
            p.Articles.Add(article);
            return await UpdateAsync(p);
        }

        public async Task<KerykeionDbResult> AddLinkAsync(Webpage page, Link link)
        {
            if (page == null || link == null)
            {
                return KerykeionDbResult.Fail();
            }

            var p = await FindByIdAllIncludedAsync(page.Id);
            if (p == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "The page to add articles returned null!" });
            }
            p.Links.Add(link);
            return await UpdateAsync(p);
        }

        /// <summary>
        /// Lists all the Webpages including their foreign keys.
        /// </summary>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing a list of webpages with their foreign keys included.
        /// </returns>
        public virtual async Task<IEnumerable<Webpage>> ListAllIncludedAsync()
        {
            return await Pages.Include(p => p.Links)
                                    .Include(p => p.Articles)
                                    .ToListAsync();
        }

        /// <summary>
        /// Lists all the Webpages including their foreign keys to a data transfer object.
        /// </summary>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing a list of webpage data transfer objects with their foreign keys included.
        /// </returns>
        public virtual async Task<IEnumerable<WebPageDto>> ListAllIncludedToDtoAsync()
        {
            var pages = await ListAllIncludedAsync();
            return pages.Select(p => new WebPageDto
            {
                Id = p.Id,
                Name = p.Name,
                Articles = p.Articles.Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                Links = p.Links.Select(l => new LinkDto
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToList()
            });
        }
    }
}
