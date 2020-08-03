using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Data;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Services;
using KerykeionDbContextExtensions;
using KerykeionStringExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly KerykeionTranslationsService _translationsService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="translationsService"></param>
        public EntitiesRepository(TContext context,
            KerykeionTranslationsService translationsService)
        {
            Context = context;
            _translationsService = translationsService;
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
        public async Task<object> FindByIdAndTableNameAsync(string id, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return null;

            var type = GetEntityTypeByTableName(tableName);
            if (type == null) return null;

            if (id.Split(",").Count() == 2)
            {
                foreach (var ID in id.Split(","))
                {
                    if (!Guid.TryParse(ID, out _)) return null;
                }

                return await Context.FindAsync(type.ClrType, Guid.Parse(id.Split(",")[0]), Guid.Parse(id.Split(",")[1]));
            }

            if (!Guid.TryParse(id, out _)) return null;

            return await Context.FindAsync(type.ClrType, Guid.Parse(id));
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
            if (InheritsFromKeryKeionBaseClass(GetEntityType(entity.GetType())))
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

            if (formDict.ContainsKey(PropertyNameConstants.Name))
            {
                var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], tableName);
                if (exists)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"The name '{formDict[PropertyNameConstants.Name]}' is already taken." });
                }
            }

            var entity = Activator.CreateInstance(type.ClrType);

            var result = await AssignPropertiesAsync(entity, formDict);
            if (!result.Successfull)
            {
                return result;
            }

            return await AddAsync(entity);
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

            if (formDict.ContainsKey(PropertyNameConstants.Name))
            {
                var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], GetTableNameByType(entity.GetType()));
                if (exists && entity.GetType().GetProperty(PropertyNameConstants.Name).GetValue(entity) != formDict[PropertyNameConstants.Name])
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"The name '{formDict[PropertyNameConstants.Name]}' is already taken." });
                }
            }

            var result = await AssignPropertiesAsync(entity, formDict);
            if (!result.Successfull)
            {
                return result;
            }

            return await UpdateAsync(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="formForeignKeys"></param>
        /// <returns></returns>
        public async Task<KerykeionDbResult> AssignForeignKeysAsync(object entity, IEnumerable<KeyValuePair<string, StringValues>> formForeignKeys)
        {
            foreach (var key in formForeignKeys)
            {
                var foreignKeyName = key.Key.Split("-").Last();

                var result = await AssignForeignKeyAsync(entity, key.Value.ToString(), foreignKeyName);

                if (result == null) continue;

                if (!result.Successfull) return result;
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

            if (InheritsFromKeryKeionBaseClass(GetEntityType(entity.GetType())))
            {
                Guid entityId = Guid.Parse(entity.GetType().GetProperty(PropertyNameConstants.Id).GetValue(entity).ToString());
                foreignKeys.ForEach(fk => lstDto.Add(new ForeignKeyDto
                {
                    Name = fk.Name,
                    Value = GetAll(GetTableNameByType(entity.GetType())).Cast<KerykeionBaseClass>().Where(e => e.Id.Equals(entityId)).Select(e => EF.Property<Guid>(e, fk.Name)).FirstOrDefault().ToString()
                }));
            }
            else
            {
                if (GetEntityType(entity.GetType()).FindPrimaryKey().Properties.Count == 2)
                {
                    foreignKeys.ForEach(fk => lstDto.Add(new ForeignKeyDto
                    {
                        Name = fk.Name,
                        Value = entity.GetType().GetProperty(fk.Name).GetValue(entity).ToString()
                    }));
                }
            }

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
            if (string.IsNullOrEmpty(tableName)) return null;

            if (GetEntityTypeByTableName(tableName) == null) return null;

            if (!GetEntitiesTableNames().Select(s => s.CompleteTrimAndUpper()).Contains(tableName.CompleteTrimAndUpper()))
                return null;

            List<EntitySideNavDto> lstEntitiesDto = new List<EntitySideNavDto>();


            if (!InheritsFromKeryKeionBaseClass(GetEntityTypeByTableName(tableName)))
            {
                var priKeyProps = GetEntityTypeByTableName(tableName).FindPrimaryKey().Properties;

                if (priKeyProps.Count == 2)
                {
                    foreach (var item in GetAll(tableName))
                    {
                        var dto = new EntitySideNavDto();

                        foreach (var prop in priKeyProps)
                        {
                            dto.Id += item.GetType().GetProperty(prop.Name).GetValue(item) + (priKeyProps.Last() == prop ? "" : ",");
                        }

                        dto.Name = dto.Id;
                        lstEntitiesDto.Add(dto);
                    }

                    return lstEntitiesDto;
                }

                return null;
            }

            await GetAll(tableName).ForEachAsync(obj => lstEntitiesDto.Add(new EntitySideNavDto
            {
                Id = obj.GetType().GetProperty(PropertyNameConstants.Id).GetValue(obj).ToString(),
                Name = obj.GetType().GetProperty(PropertyNameConstants.Name)?.GetValue(obj)?.ToString(),
                DateTimeCreated = $"{DateTime.Parse(obj.GetType().GetProperty(PropertyNameConstants.DateTimeCreated)?.GetValue(obj)?.ToString()).ToShortDateString()} - ({DateTime.Parse(obj.GetType().GetProperty(PropertyNameConstants.DateTimeCreated)?.GetValue(obj).ToString()).ToShortTimeString()})" ?? "Something went wrong wi."
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
        /// <param name="entityType"></param>
        /// <returns></returns>
        public bool InheritsFromKeryKeionBaseClass(IEntityType entityType)
        {
            if (entityType == null)
            {
                return false;
            }
            return typeof(KerykeionBaseClass).IsAssignableFrom(entityType.ClrType);
        }

        private async Task<bool> ExistsAsync(string name, string tableName)
        {
            if (!InheritsFromKeryKeionBaseClass(GetEntityTypeByTableName(tableName)))
            {
                return false;
            }
            var allEntities = await ListAllAsync(tableName);
            return allEntities.Select(o => o.GetType().GetProperty(PropertyNameConstants.UniqueNameIdentifier)?.GetValue(o)).Contains(name?.CompleteTrimAndUpper());
        }

        private async Task<KerykeionDbResult> AssignPropertiesAsync(object entity, Dictionary<string, StringValues> formDict)
        {
            var entityType = GetEntityType(entity.GetType());
            List<KerykeionDbError> errors = new List<KerykeionDbError>();

            foreach (var prop in entityType.GetProperties())
            {
                var property = entityType.ClrType.GetProperty(prop.Name);
                if (property == null)
                {
                    if (formDict.ContainsKey(prop.Name))
                    {
                        var result = await AssignForeignKeyAsync(entity, formDict[prop.Name].ToString(), prop.Name);

                        if (result == null) continue;

                        if (!result.Successfull) errors.Add(result.Errors.FirstOrDefault());
                    }

                    continue;
                }
                if (prop.Name.Equals(PropertyNameConstants.Id, StringComparison.OrdinalIgnoreCase) || prop.Name.Equals(PropertyNameConstants.DateTimeCreated, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (formDict.ContainsKey(prop.Name))
                {
                    if (prop.IsPrimaryKey())
                    {
                        if (Guid.TryParse(formDict[prop.Name], out _))
                        {
                            var navigation = entityType.GetNavigations().FirstOrDefault(n => prop.Name.Contains(n.Name, StringComparison.OrdinalIgnoreCase));
                            var targetType = navigation?.GetTargetType()?.ClrType;
                            if (targetType == null)
                            {
                                errors.Add(new KerykeionDbError { Message = $"There is no '{targetType?.Name}' found in the database with '{formDict[prop.Name]}' as primary key." });
                                continue;
                            }

                            var target = await Context.FindAsync(targetType, Guid.Parse(formDict[prop.Name]));
                            if (target == null)
                            {
                                errors.Add(new KerykeionDbError { Message = $"There is no '{targetType?.Name}' found in the database with '{formDict[prop.Name]}' as primary key." });
                                continue;
                            }

                            property.SetValue(entity, Guid.Parse(formDict[prop.Name]));
                            continue;
                        }

                        errors.Add(new KerykeionDbError
                        {
                            Message = $"Please provide a valid GUID for the property '{prop.Name}'."
                        });

                        continue;
                    }

                    if (prop.IsForeignKey())
                    {
                        var result = await AssignForeignKeyAsync(entity, formDict[prop.Name].ToString(), prop.Name);

                        if (result == null) continue;

                        if (!result.Successfull) errors.Add(result.Errors.FirstOrDefault());

                        continue;
                    }

                    if (string.IsNullOrEmpty(formDict[prop.Name]))
                    {
                        continue;
                    }

                    if (int.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, int.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (bool.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, bool.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (DateTime.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, DateTime.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (TimeSpan.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, TimeSpan.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (float.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, float.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (double.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, double.Parse(formDict[prop.Name]));
                        continue;
                    }

                    if (decimal.TryParse(formDict[prop.Name], out _))
                    {
                        property.SetValue(entity, decimal.Parse(formDict[prop.Name]));
                        continue;
                    }

                    property.SetValue(entity, formDict[prop.Name].ToString());
                }
            }

            if (errors.Count > 0) return KerykeionDbResult.Fail(errors.ToArray());

            return KerykeionDbResult.Success();
        }

        private async Task<KerykeionDbResult> AssignForeignKeyAsync(object entity, string formValue, string propertyName)
        {
            var foreignKey = GetEntityType(entity.GetType()).GetForeignKeys().FirstOrDefault(fk => fk.GetDefaultName().Contains(propertyName, StringComparison.OrdinalIgnoreCase));

            if (foreignKey == null) return null;

            if (string.IsNullOrEmpty(formValue?.Trim()))
            {
                if (foreignKey.IsRequired)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"The '{propertyName}' is required." });
                }

                Context.Entry(entity).Property(propertyName).CurrentValue = null;
                return null;
            }

            if (!Guid.TryParse(formValue, out _))
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"'{formValue}' is not a valid value for the '{propertyName}'." });
            }

            var foreignPrincipalEntityType = foreignKey?.PrincipalEntityType?.ClrType;
            if (foreignPrincipalEntityType == null) return null;

            var foreignEntity = await Context.FindAsync(foreignPrincipalEntityType, Guid.Parse(formValue));
            if (foreignEntity == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"There is no '{foreignPrincipalEntityType?.Name}' found in the database with '{formValue}' as primary key." });
            }

            Context.Entry(entity).Property(propertyName).CurrentValue = Guid.Parse(formValue);

            return KerykeionDbResult.Success();
        }

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
    }
}
