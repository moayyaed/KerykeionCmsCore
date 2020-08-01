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
    /// 
    /// </summary>
    public class EntitiesService
    {
        /// <summary>
        /// 
        /// </summary>
        protected internal IEntitiesRepository EntitiesRepo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesRepo"></param>
        public EntitiesService(IEntitiesRepository entitiesRepo)
        {
            EntitiesRepo = entitiesRepo;
        }

        #region Main CRUD funx
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IQueryable<object> GetAll(string tableName)
        {
            return EntitiesRepo.GetAll(tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> ListAllAsync(string tableName)
        {
            return await EntitiesRepo.ListAllAsync(tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<object> FindByIdAndTableNameAsync(Guid id, string tableName)
        {
            return await EntitiesRepo.FindByIdAndTableNameAsync(id, tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> DeleteAsync(object entity)
        {
            return await EntitiesRepo.DeleteAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AddAsync(object entity)
        {
            return await EntitiesRepo.AddAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AddAsync(string tableName, Dictionary<string, StringValues> formDict)
        {
            return await EntitiesRepo.AddAsync(tableName, formDict);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity)
        {
            return await EntitiesRepo.UpdateAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity, Dictionary<string, StringValues> formDict)
        {
            return await EntitiesRepo.UpdateAsync(entity, formDict);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formForeignKeys"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AssignFormForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            return await EntitiesRepo.AssignForeignKeysAsync(entity, formForeignKeys);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IProperty> GetForeignKeyPropertiesByType(Type type)
        {
            return EntitiesRepo.GetForeignKeyPropertiesByType(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IForeignKey> GetForeignKeysByType(Type type)
        {
            return EntitiesRepo.GetForeignKeysByType(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableNameByType(Type type)
        {
            return EntitiesRepo.GetTableNameByType(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntityType GetEntityType(Type type)
        {
            return EntitiesRepo.GetEntityType(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(object entity)
        {
            return EntitiesRepo.GetForeignKeyPropertiesToDto(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetEntitiesTableNames()
        {
            return EntitiesRepo.GetEntitiesTableNames();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName)
        {
            return EntitiesRepo.GetEntityPropertiesByTable(tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EntitySideNavDto>> ListAllToDtoAsync(string tableName)
        {
            return await EntitiesRepo.ListAllToDtoAsync(tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEntityType GetEntityTypeByTableName(string tableName)
        {
            return EntitiesRepo.GetEntityTypeByTableName(tableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool InheritsFromKeryKeionBaseClass(IEntityType entityType)
        {
            return EntitiesRepo.InheritsFromKeryKeionBaseClass(entityType);
        }
    }
}
