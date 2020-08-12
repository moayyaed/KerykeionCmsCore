using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for performing CRUD operations on Entities.
    /// </summary>
    public class EntitiesService
    {
        protected internal IEntitiesRepository EntitiesRepo;

        /// <summary>
        /// Creates a new instance of an EntitiesService.
        /// </summary>
        /// <param name="entitiesRepo">The repository where the entities are stored in.</param>
        public EntitiesService(IEntitiesRepository entitiesRepo)
        {
            EntitiesRepo = entitiesRepo;
        }

        #region Main CRUD funx
        /// <summary>
        /// Gets all the entities that reside in a specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table of the entities to query.</param>
        /// <returns>
        /// An Iqueryable of Entities residing in the table that matches the specified table name.
        /// </returns>
        public IQueryable<object> GetAll(string tableName)
        {
            return EntitiesRepo.GetAll(tableName);
        }

        /// <summary>
        /// Lists all the entities specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// a list of entities specified by the table name.
        /// </returns>
        public async Task<IEnumerable<object>> ListAllAsync(string tableName)
        {
            return await EntitiesRepo.ListAllAsync(tableName);
        }

        /// <summary>
        /// Lists all the entities, reformed to data transfer objects, specified by the table name.
        /// </summary>
        /// <param name="tableName">The name of the table the entities reside in.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// a list of EntitySideNavDto's specified by the table name.
        /// </returns>
        public async Task<IEnumerable<EntitySideNavDto>> ListAllToDtoAsync(string tableName)
        {
            return await EntitiesRepo.ListAllToDtoAsync(tableName);
        }

        /// <summary>
        /// Searches for an entity specified by an ID and table name.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// the entity specified by the table name and the specified ID.</returns>
        public async Task<object> FindByIdAndTableNameAsync(string id, string tableName)
        {
            return await EntitiesRepo.FindByIdAndTableNameAsync(id, tableName);
        }

        /// <summary>
        /// Creates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> CreateAsync(object entity)
        {
            return await EntitiesRepo.CreateAsync(entity);
        }

        /// <summary>
        /// Creates an Entity specified by its table with the form dictionary values, as an asynchronous operation.
        /// </summary>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <param name="formDict">The form dictionary containing the values to create the entity.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> CreateAsync(string tableName, Dictionary<string, StringValues> formDict)
        {
            return await EntitiesRepo.CreateAsync(tableName, formDict);
        }

        /// <summary>
        /// Updates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to update.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity)
        {
            return await EntitiesRepo.UpdateAsync(entity);
        }

        /// <summary>
        /// Updates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to update.</param>
        /// <param name="formDict">The form dictionary containing the new values for the specified Entity.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity, Dictionary<string, StringValues> formDict)
        {
            return await EntitiesRepo.UpdateAsync(entity, formDict);
        }

        /// <summary>
        /// Deletes the specified Entity from the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The Entity to delete.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> DeleteAsync(object entity)
        {
            return await EntitiesRepo.DeleteAsync(entity);
        }

        /// <summary>
        /// Sets Foreign Key property values to the specified Entity.
        /// </summary>
        /// <param name="entity">The Entity to set the foreign keys value to.</param>
        /// <param name="formForeignKeys">The form dictionary foreign keys and values.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> SetForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            return await EntitiesRepo.SetForeignKeysAsync(entity, formForeignKeys);
        }
        #endregion


        /// <summary>
        /// Gets the IEntityType of the specified ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the IEntityType from.</param>
        /// <returns>
        /// An IEntityType specified by the given ClrType.
        /// </returns>
        public IEntityType FindEntityTypeByClrType(Type type)
        {
            return EntitiesRepo.FindEntityTypeByClrType(type);
        }

        /// <summary>
        /// Gets the IEntityType of the specified table name.
        /// </summary>
        /// <param name="tableName">The table name to get the IEntityType from.</param>
        /// <returns>
        /// An IEntityType specified by the given table name.
        /// </returns>
        public IEntityType FindEntityTypeByTableName(string tableName)
        {
            return EntitiesRepo.FindEntityTypeByTableName(tableName);
        }

        /// <summary>
        /// Gets the table name of the specified ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the table name from.</param>
        /// <returns>
        /// A string representation of the table name of the specified ClrType.
        /// </returns>
        public string FindTableNameByClrType(Type type)
        {
            return EntitiesRepo.FindTableNameByClrType(type);
        }

        /// <summary>
        /// Gets all the tablenames of the current DB Context entities.
        /// </summary>
        /// <returns>
        /// A IEnumerable of table names.
        /// </returns>
        public IEnumerable<string> GetEntitiesTableNames()
        {
            return EntitiesRepo.GetEntitiesTableNames();
        }

        /// <summary>
        /// Gets all the database model properties specified by the given table name.
        /// </summary>
        /// <param name="tableName">The name of the table to get the properties from.</param>
        /// <returns>
        /// A IEnumerable of IProperty.
        /// </returns>
        public IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName)
        {
            return EntitiesRepo.GetEntityPropertiesByTable(tableName);
        }

        /// <summary>
        /// Gets all the database model foreign key properties specified by the given ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the foreign key properties from.</param>
        /// <returns>
        /// A IEnumerable of IProperty where the IProperty is a foreign key.
        /// </returns>
        public IEnumerable<IProperty> GetForeignKeyPropertiesByClrType(Type type)
        {
            return EntitiesRepo.GetForeignKeyPropertiesByClrType(type);
        }

        /// <summary>
        /// Gets all the database model foreign key properties, reformed to data transfer objects, specified by the given entity.
        /// </summary>
        /// <param name="entity">The entity to get the foreign key properties from.</param>
        /// <returns>
        /// A IEnumerable of ForeignKeyDto.
        /// </returns>
        public IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(object entity)
        {
            return EntitiesRepo.GetForeignKeyPropertiesToDto(entity);
        }

        /// <summary>
        /// Gets all the database model foreign keys specified by the given ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the foreign keys from.</param>
        /// <returns>
        /// A IEnumerable of IForeignKey.
        /// </returns>
        public IEnumerable<IForeignKey> GetForeignKeysByClrType(Type type)
        {
            return EntitiesRepo.GetForeignKeysByClrType(type);
        }

        /// <summary>
        /// Gets a flag Indicating wether the given IEntityType inherits from the KerykeionBaseClass.
        /// </summary>
        /// <param name="entityType">The IEntityType to check if it inherits from the KerykeionBaseClass.</param>
        /// <returns>
        /// true if the IEntityType inherits from KerykeionBaseClass.
        /// </returns>
        public bool InheritsFromKeryKeionBaseClass(IEntityType entityType)
        {
            return EntitiesRepo.InheritsFromKeryKeionBaseClass(entityType);
        }
    }
}
