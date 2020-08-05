using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Dtos;
using KerykeionStringExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for delivering CRUD operations for entities in a Database.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to use this service for.</typeparam>
    public class KerykeionCmsService<TEntity>
        where TEntity : KerykeionBaseClass
    {
        protected readonly EntitiesService EntitiesService;

        /// <summary>
        /// Constructs a new instance of KerykeionCms.Services.KerykeionCmsService
        /// </summary>
        /// <param name="entitiesService">The entities service the KerykeionCmsService will operate over.</param>
        public KerykeionCmsService(EntitiesService entitiesService)
        {
            EntitiesService = entitiesService;
        }

        private string TableName => EntitiesService.GetTableNameByType(typeof(TEntity));

        /// <summary>
        /// Gets all the TEntities.
        /// </summary>
        /// <returns>
        /// An IQueryable TEntities.
        /// </returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return EntitiesService.GetAll(TableName).Cast<TEntity>();
        }

        /// <summary>
        /// Lists all the TEntities.
        /// </summary>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, a list TEntities.
        /// </returns>
        public virtual async Task<IEnumerable<TEntity>> ListAllAsync()
        {
            var lst = await EntitiesService.ListAllAsync(TableName);
            return lst.Cast<TEntity>();
        }

        /// <summary>
        /// Finds and returns an entity, if any, who has the specified id.
        /// </summary>
        /// <param name="id">
        /// The ID to search for.
        /// </param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the entity matching the specified id.
        /// </returns>
        public virtual async Task<TEntity> FindByIdAsync(string id)
        {
            var entity = await EntitiesService.FindByIdAndTableNameAsync(id, TableName);
            return entity as TEntity;
        }

        /// <summary>
        /// Finds and returns an entity, if any, who has the specified name.
        /// </summary>
        /// <param name="name">
        /// The name to search for.
        /// </param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the entity matching the specified id.
        /// </returns>
        public virtual async Task<TEntity> FindByNameAsync(string name)
        {
            var entities = await EntitiesService.ListAllAsync(TableName);
            return entities.Cast<TEntity>().FirstOrDefault(e => e.UniqueNameIdentifier == name.CompleteTrimAndUpper());
        }

        /// <summary>
        /// Creates the specified entity in the database, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the KerykeionCms.Services.KerykeionDbResult of the operation.
        /// </returns>
        public virtual async Task<KerykeionDbResult> CreateAsync(TEntity entity)
        {
            return await EntitiesService.AddAsync(entity);
        }

        /// <summary>
        /// Creates the specified entity in the database, as an asynchronous operation.
        /// </summary>
        /// <param name="formDict">A dictionary of form keys and values.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the KerykeionCms.Services.KerykeionDbResult of the operation.
        /// </returns>
        public virtual async Task<KerykeionDbResult> CreateAsync(Dictionary<string, StringValues> formDict)
        {
            return await EntitiesService.AddAsync(TableName, formDict);
        }

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the KerykeionCms.Services.KerykeionDbResult of the operation.
        /// </returns>
        public virtual async Task<KerykeionDbResult> UpdateAsync(TEntity entity)
        {
            return await EntitiesService.UpdateAsync(entity);
        }

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="formDict">A dictionary of form keys and values.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the KerykeionCms.Services.KerykeionDbResult of the operation.
        /// </returns>
        public virtual async Task<KerykeionDbResult> UpdateAsync(TEntity entity, Dictionary<string, StringValues> formDict)
        {
            return await EntitiesService.UpdateAsync(entity, formDict);
        }

        public virtual async Task<bool> ExistsAsync(string name)
        {
            var allEntities = await ListAllAsync();
            return allEntities.Select(p => p.UniqueNameIdentifier).Contains(name?.CompleteTrimAndUpper());
        }

        /// <summary>
        /// Deletes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the KerykeionCms.Services.KerykeionDbResult of the operation.
        /// </returns>
        public virtual async Task<KerykeionDbResult> DeleteAsync(TEntity entity)
        {
            return await EntitiesService.DeleteAsync(entity);
        }
        /// <summary>
        /// Gets the foreign key properties (also shadow properties) for the current TEntity.
        /// </summary>
        /// <returns>
        /// An IEnumerable of foreign keys.
        /// </returns>
        public IEnumerable<IProperty> GetForeignKeyProperties()
        {
            return EntitiesService.GetForeignKeyPropertiesByType(typeof(TEntity));
        }
        /// <summary>
        /// Gets the foreign key properties (also shadow properties) and its values for the current TEntity.
        /// </summary>
        /// <param name="entity">
        /// The TEntity to get the foreignkeys info from.
        /// </param>
        /// <returns>
        /// An IEnumerable of a data transfer object containing the name and value of the foreign key properties.
        /// </returns>
        public IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(TEntity entity)
        {
            return EntitiesService.GetForeignKeyPropertiesToDto(entity);
        }
    }
}
