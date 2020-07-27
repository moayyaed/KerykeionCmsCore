using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Repositories
{
    /// <summary>
    /// Provides an abstraction for a repository for Database Context Entities.
    /// </summary>
    public interface IEntitiesRepository
    {
        /// <summary>
        /// Gets all the entities specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>All the entities in the specified table.</returns>
        IQueryable<object> GetAll(string tableName);
        /// <summary>
        /// Lists all the entities specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// a list of entities specified by the table name.</returns>
        Task<IEnumerable<object>> ListAllAsync(string tableName);
        /// <summary>
        /// Gets all the tablenames of the current DB Context entities.
        /// </summary>
        /// <returns>A IEnumerable of table names.</returns>
        IEnumerable<string> GetEntitiesTableNames();
        /// <summary>
        /// Searches for an entity specified by an ID and table name.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// the entity specified by the table name and the specified ID.</returns>
        Task<object> FindByIdAndTableNameAsync(Guid id, string tableName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> UpdateAsync(object entity, Dictionary<string, StringValues> formDict);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> UpdateAsync(object entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> DeleteAsync(object entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> AddAsync(object entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> AddAsync(string tableName, Dictionary<string, StringValues> formDict);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formForeignKeys"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> AssignFormForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<IEnumerable<EntitySideNavDto>> ListAllToDtoAsync(string tableName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<IProperty> GetForeignKeyPropertiesByType(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<IForeignKey> GetForeignKeysByType(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(object entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool InheritsFromKeryKeionBaseClass(string tableName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IEntityType GetEntityTypeByTableName(string tableName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTableNameByType(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEntityType GetEntityType(Type type);
    }
}
