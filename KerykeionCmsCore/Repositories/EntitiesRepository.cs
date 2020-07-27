using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Data;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KerykeionStringExtensions;
using KerykeionDbContextExtensions;

namespace KerykeionCmsCore.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class EntitiesRepository<TContext, TUser> : IEntitiesRepository
        where TUser : KerykeionUser
        where TContext : KerykeionCmsDbContext<TUser>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly TContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public EntitiesRepository(TContext context)
        {
            Context = context;
        }


        #region Main CRUD funx
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IQueryable<object> GetAll(string tableName)
        {
            return Context.Set(GetEntityTypeByTableName(tableName)?.ClrType).AsNoTracking();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> ListAllAsync(string tableName)
        {
            return await GetAll(tableName).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<object> FindByIdAndTableNameAsync(Guid id, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            var type = GetEntityTypeByTableName(tableName);
            if (type == null)
            {
                return null;
            }

            return await Context.FindAsync(type.ClrType, id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> DeleteAsync(object entity)
        {
            Context.Remove(entity);
            return await TrySaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AddAsync(object entity)
        {
            entity.GetType().GetProperty(PropertyNameConstants.DateTimeCreated).SetValue(entity, DateTime.Now);
            Context.Add(entity);
            return await TrySaveChangesAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AddAsync(string tableName, Dictionary<string, StringValues> formDict)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Please specify a valid table name." });
            }

            var type = GetEntityTypeByTableName(tableName);
            if (type == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"There are no entities in a table called '{tableName}'." });
            }

            var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], tableName);
            if (exists)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"The name '{formDict[PropertyNameConstants.Name]}' is already taken." });
            }

            var activatedEntity = Activator.CreateInstance(type.ClrType) as KerykeionBaseClass;
            activatedEntity.DateTimeCreated = DateTime.Now;

            foreach (var prop in type.GetProperties())
            {
                var property = type.ClrType.GetProperties().FirstOrDefault(p => p.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                if (property == null)
                {
                    var foreignKey = type.GetForeignKeys().FirstOrDefault(fk => fk.GetDefaultName().Contains(prop.Name, StringComparison.OrdinalIgnoreCase));
                    if (foreignKey == null)
                    {
                        continue;
                    }

                    if (formDict.ContainsKey(prop.Name))
                    {
                        if (string.IsNullOrEmpty(formDict[prop.Name].ToString()?.Trim()))
                        {
                            continue;
                        }

                        if (!Guid.TryParse(formDict[prop.Name].ToString(), out _))
                        {
                            return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"'{formDict[prop.Name]}' is not a valid value for the '{prop.Name}'. Either provide a valid GUID or make sure the input is completely empty." });
                        }

                        var foreignPrincipalEntityType = foreignKey?.PrincipalEntityType?.ClrType;
                        if (foreignPrincipalEntityType == null) continue;

                        var foreignEntity = await Context.FindAsync(foreignPrincipalEntityType, Guid.Parse(formDict[prop.Name].ToString()));
                        if (foreignEntity == null)
                        {
                            return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"There is no '{foreignPrincipalEntityType?.Name}' found in the database with '{formDict[prop.Name]}' as primary key." });
                        }

                        Context.Entry(activatedEntity).Property(prop.Name).CurrentValue = Guid.Parse(formDict[prop.Name].ToString());
                    }

                    continue;
                }
                if (prop.Name.Equals(PropertyNameConstants.Id, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (formDict.ContainsKey(prop.Name))
                {
                    if (string.IsNullOrEmpty(formDict[prop.Name]))
                    {
                        continue;
                    }

                    if (int.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(activatedEntity, int.Parse(formDict[prop.Name]));
                        continue;
                    }

                    property.SetValue(activatedEntity, formDict[prop.Name].ToString());
                }
            }

            return await AddAsync(activatedEntity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await TrySaveChangesAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> UpdateAsync(object entity, Dictionary<string, StringValues> formDict)
        {
            if (entity == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"Please provide a valid entity when calling this function." });
            }

            var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], GetTableNameByType(entity.GetType()));
            if (exists && entity.GetType().GetProperty(PropertyNameConstants.Name).GetValue(entity) != formDict[PropertyNameConstants.Name])
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"The name '{formDict[PropertyNameConstants.Name]}' is already taken." });
            }

            foreach (var keyValue in formDict)
            {
                var property = entity.GetType().GetProperty(keyValue.Key);
                if (property == null)
                {
                    var foreignKey = GetForeignKeysByType(entity.GetType()).FirstOrDefault(fk => fk.GetDefaultName().Contains(keyValue.Key, StringComparison.OrdinalIgnoreCase));
                    if (foreignKey == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(keyValue.Value.ToString()?.Trim()))
                    {
                        Context.Entry(entity).Property(keyValue.Key).CurrentValue = null;
                        continue;
                    }

                    if (!Guid.TryParse(formDict[keyValue.Key].ToString(), out _))
                    {
                        return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"'{keyValue.Value}' is not a valid value for the '{keyValue.Key}'. Either provide a valid GUID or make sure the input is completely empty." });
                    }

                    var foreignPrincipalEntityType = foreignKey?.PrincipalEntityType?.ClrType;
                    if (foreignPrincipalEntityType == null) continue;

                    var foreignEntity = await Context.FindAsync(foreignPrincipalEntityType, Guid.Parse(keyValue.Value.ToString()));
                    if (foreignEntity == null)
                    {
                        return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"There is no '{foreignPrincipalEntityType?.Name}' found in the database with '{keyValue.Value}' as primary key." });
                    }

                    Context.Entry(entity).Property(keyValue.Key).CurrentValue = Guid.Parse(keyValue.Value.ToString());

                    continue;
                }

                if (keyValue.Key.Equals(PropertyNameConstants.Id, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (keyValue.Key.Equals(PropertyNameConstants.DateTimeCreated, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(keyValue.Value))
                {
                    continue;
                }

                if (int.TryParse(keyValue.Value, out _))
                {
                    property.SetValue(entity, int.Parse(formDict[keyValue.Key]));
                    continue;
                }

                property.SetValue(entity, keyValue.Value.ToString());
            }


            Context.Entry(entity).State = EntityState.Modified;
            return await TrySaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formForeignKeys"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AssignFormForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            foreach (var key in formForeignKeys)
            {
                var foreignKeyName = key.Key.Split("-").Last();
                var foreignKey = GetForeignKeysByType(entity.GetType()).FirstOrDefault(fk => fk.GetDefaultName().Contains(foreignKeyName, StringComparison.OrdinalIgnoreCase));
                if (foreignKey == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(key.Value.ToString()?.Trim()))
                {
                    Context.Entry(entity).Property(foreignKeyName).CurrentValue = null;
                    continue;
                }

                if (!Guid.TryParse(key.Value.ToString(), out _))
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"'{key.Value}' is not a valid value for the '{foreignKeyName}'. Either provide a valid GUID or make sure the input is completely empty." });
                }

                var foreignPrincipalEntityType = foreignKey?.PrincipalEntityType?.ClrType;
                if (foreignPrincipalEntityType == null) continue;

                var foreignEntity = await Context.FindAsync(foreignPrincipalEntityType, Guid.Parse(key.Value.ToString()));
                if (foreignEntity == null)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"There is no '{foreignPrincipalEntityType?.Name}' found in the database with '{key.Value}' as primary key." });
                }

                Context.Entry(entity).Property(foreignKeyName).CurrentValue = Guid.Parse(key.Value.ToString());
            }

            return KerykeionDbResult.Success();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IProperty> GetForeignKeyPropertiesByType(Type type)
        {
            return GetEntityType(type).GetProperties().Where(p => p.IsForeignKey());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IForeignKey> GetForeignKeysByType(Type type)
        {
            return GetEntityType(type).GetForeignKeys();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableNameByType(Type type)
        {
            return GetEntityType(type)?.GetTableName();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntityType GetEntityType(Type type)
        {
            return Context.Model.FindEntityType(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<ForeignKeyDto> GetForeignKeyPropertiesToDto(object entity)
        {
            var foreignKeys = GetForeignKeyPropertiesByType(entity.GetType()).ToList();
            List<ForeignKeyDto> lstDto = new List<ForeignKeyDto>();

            Guid entityId = Guid.Parse(entity.GetType().GetProperty(PropertyNameConstants.Id).GetValue(entity).ToString());

            foreignKeys.ForEach(fk => lstDto.Add(new ForeignKeyDto
            {
                Name = fk.Name,
                Value = GetAll(GetTableNameByType(entity.GetType())).Cast<KerykeionBaseClass>().Where(e => e.Id.Equals(entityId)).Select(e => EF.Property<Guid>(e, fk.Name)).FirstOrDefault().ToString()
            }));

            return lstDto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetEntitiesTableNames()
        {
            List<string> defaultEntities = new List<string>
            {
                Context.Users.GetType().FullName,
                Context.Roles.GetType().FullName,
                Context.UserClaims.GetType().FullName,
                Context.UserLogins.GetType().FullName,
                Context.UserRoles.GetType().FullName,
                Context.UserTokens.GetType().FullName,
                Context.RoleClaims.GetType().FullName,
                Context.Webpages.GetType().FullName,
                Context.Links.GetType().FullName,
                Context.Articles.GetType().FullName,
                Context.Images.GetType().FullName
            };

            return Context.Model.GetEntityTypes().Where(e => !defaultEntities.Any(des => des.Contains(e.ClrType.FullName, StringComparison.OrdinalIgnoreCase))).Select(e => e.GetTableName());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName) || !GetEntitiesTableNames().Contains(tableName))
            {
                return null;
            }
            var et = GetEntityTypeByTableName(tableName);
            if (et == null)
            {
                return null;
            }
            return et.GetProperties();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EntitySideNavDto>> ListAllToDtoAsync(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            if (GetEntityTypeByTableName(tableName) == null)
            {
                return null;
            }

            if (!GetEntitiesTableNames().Select(s => s.CompleteTrimAndUpper()).Contains(tableName.CompleteTrimAndUpper()))
            {
                return null;
            }

            if (!InheritsFromKeryKeionBaseClass(tableName))
            {
                return null;
            }

            List<EntitySideNavDto> lstEntitiesDto = new List<EntitySideNavDto>();
            await GetAll(tableName).ForEachAsync(obj => lstEntitiesDto.Add(new EntitySideNavDto
            {
                Id = Guid.Parse(obj.GetType().GetProperty("Id")?.GetValue(obj)?.ToString()),
                Name = obj.GetType().GetProperty("Name")?.GetValue(obj)?.ToString(),
                DateTimeCreated = $"{DateTime.Parse(obj.GetType().GetProperty("DateTimeCreated")?.GetValue(obj)?.ToString()).ToShortDateString()} - ({DateTime.Parse(obj.GetType().GetProperty("DateTimeCreated")?.GetValue(obj).ToString()).ToShortTimeString()})" ?? "Something went wrong wi."
            }));

            return lstEntitiesDto.OrderBy(e => e.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEntityType GetEntityTypeByTableName(string tableName)
        {
            return Context.Model.GetEntityTypes().FirstOrDefault(et => string.Equals(et.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task<KerykeionDbResult> TrySaveChangesAsync(object entity = null)
        {
            try
            {
                await Context.SaveChangesAsync();
                return KerykeionDbResult.Success(entity);
            }
            catch (Exception ex)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError
                {
                    Message = ex.InnerException.Message
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool InheritsFromKeryKeionBaseClass(string tableName)
        {
            if (GetEntityTypeByTableName(tableName) == null)
            {
                return false;
            }
            return typeof(KerykeionBaseClass).IsAssignableFrom(GetEntityTypeByTableName(tableName).ClrType);
        }

        private async Task<bool> ExistsAsync(string name, string tableName)
        {
            var allEntities = await ListAllAsync(tableName);
            return allEntities.Select(o => o.GetType().GetProperty(PropertyNameConstants.UniqueNameIdentifier)?.GetValue(o)).Contains(name?.CompleteTrimAndUpper());
        }
    }
}
