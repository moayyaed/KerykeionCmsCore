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
        /// Gets all the entities that reside in a specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table of the entities to query.</param>
        /// <returns>
        /// An Iqueryable of Entities residing in the table that matches the specified table name.
        /// </returns>
        IQueryable<object> GetAll(string tableName);

        /// <summary>
        /// Lists all the entities specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// a list of entities specified by the table name.
        /// </returns>
        Task<IEnumerable<object>> ListAllAsync(string tableName);

        /// <summary>
        /// Lists all the entities, reformed to data transfer objects, specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// a list of EntitySideNavDto's specified by the table name.
        /// </returns>
        Task<IEnumerable<EntitySideNavDto>> ListAllToDtoAsync(string tableName);

        /// <summary>
        /// Searches for an entity specified by an ID and table name.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// the entity specified by the table name and the specified ID.</returns>
        Task<object> FindByIdAndTableNameAsync(string id, string tableName);

        /// <summary>
        /// Creates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> CreateAsync(object entity);

        /// <summary>
        /// Creates an Entity specified by its table with the form dictionary values, as an asynchronous operation.
        /// </summary>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <param name="formDict">The form dictionary containing the values to create the entity.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> CreateAsync(string tableName, Dictionary<string, StringValues> formDict);

        /// <summary>
        /// Updates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to update.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> UpdateAsync(object entity);

        /// <summary>
        /// Updates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to update.</param>
        /// <param name="formDict">The form dictionary containing the new values for the specified Entity.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> UpdateAsync(object entity, Dictionary<string, StringValues> formDict);

        /// <summary>
        /// Deletes the specified Entity from the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to delete.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> DeleteAsync(object entity);

        /// <summary>
        /// Sets Foreign Key property values to the specified Entity.
        /// </summary>
        /// <param name="entity">The Entity to set the foreign keys value to.</param>
        /// <param name="formForeignKeys">The form dictionary foreign keys and values.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        Task<KerykeionDbResult> SetForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys);

        /// <summary>
        /// Gets the IEntityType of the specified ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the IEntityType from.</param>
        /// <returns>
        /// An IEntityType specified by the given ClrType.
        /// </returns>
        IEntityType FindEntityTypeByClrType(Type type);

        /// <summary>
        /// Gets the IEntityType of the specified table name.
        /// </summary>
        /// <param name="tableName">The table name to get the IEntityType from.</param>
        /// <returns>
        /// An IEntityType specified by the given table name.
        /// </returns>
        IEntityType FindEntityTypeByTableName(string tableName);

        /// <summary>
        /// Gets the table name of the specified ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the table name from.</param>
        /// <returns>
        /// A string representation of the table name of the specified ClrType.
        /// </returns>
        string FindTableNameByClrType(Type type);

        /// <summary>
        /// Gets all the tablenames of the current DB Context entities.
        /// </summary>
        /// <returns>
        /// A IEnumerable of table names.
        /// </returns>
        IEnumerable<string> GetEntitiesTableNames();

        /// <summary>
        /// Gets all the database model properties specified by the given table name.
        /// </summary>
        /// <param name="tableName">The name of the table to get the properties from.</param>
        /// <returns>
        /// A IEnumerable of IProperty.
        /// </returns>
        IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName);

        /// <summary>
        /// Gets all the database model foreign key properties specified by the given ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the foreign key properties from.</param>
        /// <returns>
        /// A IEnumerable of IProperty where the IProperty is a foreign key.
        /// </returns>
        IEnumerable<IProperty> GetForeignKeyPropertiesByClrType(Type type);

        /// <summary>
        /// Gets all the database model foreign key properties, reformed to data transfer objects, specified by the given entity.
        /// </summary>
        /// <param name="entity">The entity to get the foreign key properties from.</param>
        /// <returns>
        /// A IEnumerable of ForeignKeyDto.
        /// </returns>
        IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(object entity);

        /// <summary>
        /// Gets all the database model foreign keys specified by the given ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the foreign keys from.</param>
        /// <returns>
        /// A IEnumerable of IForeignKey.
        /// </returns>
        IEnumerable<IForeignKey> GetForeignKeysByClrType(Type type);

        /// <summary>
        /// Gets a flag Indicating wether the given IEntityType inherits from the KerykeionBaseClass.
        /// </summary>
        /// <param name="entityType">The IEntityType to check if it inherits from the KerykeionBaseClass.</param>
        /// <returns>
        /// True if the IEntityType inherits from KerykeionBaseClass.
        /// </returns>
        bool InheritsFromKeryKeionBaseClass(IEntityType entityType);
    }
}
