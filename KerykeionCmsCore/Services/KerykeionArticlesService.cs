using KerykeionCmsCore.Classes;
using KerykeionStringExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for delivering CRUD operations on Articles.
    /// </summary>
    public class KerykeionArticlesService : KerykeionCmsService<Article>
    {
        /// <summary>
        /// Constructs a new instance of KerykeionCms.Services.KerykeionArticlesService
        /// </summary>
        /// <param name="service">The entities service the KerykeionArticlesService will operate over.</param>
        public KerykeionArticlesService(EntitiesService service) : base(service)
        {
        }

        /// <summary>
        /// Gets all the articles in the database to query.
        /// </summary>
        public IQueryable<Article> Articles => GetAll();

        /// <summary>
        /// Finds and returns an article, all included, if any, which has the specified id.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>
        /// A System.Threading.Task.Task that represents the result of the asynchronous query containing the all included article with the specified id.
        /// </returns>
        public async Task<Article> FindByIdAllIncludedAsync(Guid? id)
        {
            return await GetAll().Include(a => a.Webpage)
                                    .Include(a => a.Images)
                                    .FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        /// <summary>
        /// Lists all the articles everthing included.
        /// </summary>
        /// <returns>
        /// A System.Threading.Task.Task that represents the result of the asynchronous query containing a list of articles everthing included.
        /// </returns>
        public async Task<IEnumerable<Article>> ListAllInlcudedAsync()
        {
            return await Articles.Include(a => a.Webpage)
                                    .Include(a => a.Images)
                                    .ToListAsync();
        }

        public async Task<bool> VerifyTitleAsync(string title, Guid articleId)
        {
            var article = await FindByIdAsync(articleId);
            if (article != null)
            {
                if (article.Name.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            var titles = await GetAll().Select(a => a.Name.CompleteTrimAndUpper()).ToListAsync();
            return titles.Contains(title.CompleteTrimAndUpper());
        }
    }
}
