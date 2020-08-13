using ImageManagement.Services;
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
    /// Provides the API's for performing CRUD operations on Entities.
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext the entities reside in.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class EntitiesRepository<TContext, TUser> : IEntitiesRepository
        where TUser : KerykeionUser
        where TContext : KerykeionCmsDbContext<TUser>
    {
        protected readonly TContext Context;
        private readonly KerykeionTranslationsService _translationsService;
        private readonly ImagesService _imagesService;

        /// <summary>
        /// Creates a new instance of the EntitiesRepository.
        /// </summary>
        /// <param name="context">The DbContext to be used.</param>
        /// <param name="translationsService">The service to be used for translations.</param>
        /// <param name="imagesService">The service used for image upload and removal.</param>
        public EntitiesRepository(TContext context,
            KerykeionTranslationsService translationsService,
            ImagesService imagesService)
        {
            Context = context;
            _translationsService = translationsService;
            _imagesService = imagesService;
        }

        /// <summary>
        /// Gets all the entities that reside in a specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table of the entities to query.</param>
        /// <returns>
        /// An Iqueryable of Entities residing in the table that matches the specified table name.
        /// </returns>
        public IQueryable<object> GetAll(string tableName)
        {
            return Context.Set(FindEntityTypeByTableName(tableName)?.ClrType).AsNoTracking();
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
            return await GetAll(tableName).ToListAsync();
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
            if (string.IsNullOrEmpty(tableName)) return null;

            if (FindEntityTypeByTableName(tableName) == null) return null;

            if (!GetEntitiesTableNames().Select(s => s.CompleteTrimAndUpper()).Contains(tableName.CompleteTrimAndUpper()))
                return null;

            List<EntitySideNavDto> lstEntitiesDto = new List<EntitySideNavDto>();


            if (!InheritsFromKeryKeionBaseClass(FindEntityTypeByTableName(tableName)))
            {
                var priKeyProps = FindEntityTypeByTableName(tableName).FindPrimaryKey().Properties;

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
        /// Searches for an entity specified by an ID and table name.
        /// </summary>
        /// <param name="id">The ID of the entity to search for.</param>
        /// <param name="tableName">The name of the table the entity resides in.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// the entity specified by the table name and the specified ID.</returns>
        public async Task<object> FindByIdAndTableNameAsync(string id, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return null;

            var type = FindEntityTypeByTableName(tableName);
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
        /// Creates the specified Entity in the backing repository, as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation, containing the KerykeionDbResult of the operation.
        /// </returns>
        public async Task<KerykeionDbResult> CreateAsync(object entity)
        {
            if (InheritsFromKeryKeionBaseClass(FindEntityTypeByClrType(entity.GetType())))
                entity.GetType().GetProperty(PropertyNameConstants.DateTimeCreated).SetValue(entity, DateTime.Now);

            Context.Add(entity);
            return await TrySaveChangesAsync(entity);
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
            if (string.IsNullOrEmpty(tableName))
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"{await _translationsService.TranslateAsync("Please specify a valid table name")}." });
            }

            var type = FindEntityTypeByTableName(tableName);
            if (type == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.TableNotExists, $"There are no entities in a table called '{tableName}'.", tableName) });
            }

            if (formDict.ContainsKey(PropertyNameConstants.Name))
            {
                var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], tableName);
                if (exists)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NameDuplicate, $"The name '{formDict[PropertyNameConstants.Name]}' is already taken.", formDict[PropertyNameConstants.Name]) });
                }
            }

            var entity = Activator.CreateInstance(type.ClrType);

            var result = await SetPropertiesAsync(entity, formDict);
            if (!result.Successfull)
            {
                return result;
            }

            return await CreateAsync(entity);
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
            Context.Entry(entity).State = EntityState.Modified;
            return await TrySaveChangesAsync(entity);
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
            if (entity == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = $"{await _translationsService.TranslateAsync("Please provide a valid entity when calling this function")}." });
            }

            if (formDict.ContainsKey(PropertyNameConstants.Name))
            {
                var exists = await ExistsAsync(formDict[PropertyNameConstants.Name], FindTableNameByClrType(entity.GetType()));
                if (exists && entity.GetType().GetProperty(PropertyNameConstants.Name).GetValue(entity) != formDict[PropertyNameConstants.Name])
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NameDuplicate, $"The name '{formDict[PropertyNameConstants.Name]}' is already taken.", formDict[PropertyNameConstants.Name]) });
                }
            }

            var result = await SetPropertiesAsync(entity, formDict);
            if (!result.Successfull)
            {
                return result;
            }

            return await UpdateAsync(entity);
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
            if (entity is Image)
            {
                var removeImgResult = await _imagesService.DeleteImage(entity.GetType().GetProperty("Url").GetValue(entity)?.ToString()?.Split("/")?.Last());
                if (!removeImgResult.Success)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Could not delete the image." });
                }
            }

            Context.Remove(entity);
            return await TrySaveChangesAsync();
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
            foreach (var key in formForeignKeys)
            {
                var foreignKeyName = key.Key.Split("-").Last();

                var result = await SetForeignKeyAsync(entity, key.Value.ToString(), foreignKeyName);

                if (result == null) continue;

                if (!result.Successfull) return result;
            }

            return KerykeionDbResult.Success();
        }

        /// <summary>
        /// Gets the IEntityType of the specified ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the IEntityType from.</param>
        /// <returns>
        /// An IEntityType specified by the given ClrType.
        /// </returns>
        public IEntityType FindEntityTypeByClrType(Type type)
        {
            return Context.Model.FindEntityType(type);
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
            return Context.Model.GetEntityTypes().FirstOrDefault(et => string.Equals(et.GetTableName(), tableName, StringComparison.OrdinalIgnoreCase));
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
            return FindEntityTypeByClrType(type)?.GetTableName();
        }

        /// <summary>
        /// Gets all the tablenames of the current DB Context entities.
        /// </summary>
        /// <returns>
        /// A IEnumerable of table names.
        /// </returns>
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
        /// Gets all the database model properties specified by the given table name.
        /// </summary>
        /// <param name="tableName">The name of the table to get the properties from.</param>
        /// <returns>
        /// A IEnumerable of IProperty.
        /// </returns>
        public IEnumerable<IProperty> GetEntityPropertiesByTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName) || !GetEntitiesTableNames().Contains(tableName))
            {
                return null;
            }
            var et = FindEntityTypeByTableName(tableName);
            if (et == null)
            {
                return null;
            }
            return et.GetProperties();
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
            return FindEntityTypeByClrType(type).GetProperties().Where(p => p.IsForeignKey());
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
            var foreignKeys = GetForeignKeyPropertiesByClrType(entity.GetType()).ToList();
            List<ForeignKeyDto> lstDto = new List<ForeignKeyDto>();

            if (InheritsFromKeryKeionBaseClass(FindEntityTypeByClrType(entity.GetType())))
            {
                Guid entityId = Guid.Parse(entity.GetType().GetProperty(PropertyNameConstants.Id).GetValue(entity).ToString());
                foreignKeys.ForEach(fk => lstDto.Add(new ForeignKeyDto
                {
                    Name = fk.Name,
                    Value = GetAll(FindTableNameByClrType(entity.GetType())).Cast<KerykeionBaseClass>().Where(e => e.Id.Equals(entityId)).Select(e => EF.Property<Guid>(e, fk.Name)).FirstOrDefault().ToString()
                }));
            }
            else
            {
                if (FindEntityTypeByClrType(entity.GetType()).FindPrimaryKey().Properties.Count == 2)
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
        /// Gets all the database model foreign keys specified by the given ClrType.
        /// </summary>
        /// <param name="type">The ClrType to get the foreign keys from.</param>
        /// <returns>
        /// A IEnumerable of IForeignKey.
        /// </returns>
        public IEnumerable<IForeignKey> GetForeignKeysByClrType(Type type)
        {
            return FindEntityTypeByClrType(type).GetForeignKeys();
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
            if (entityType == null)
            {
                return false;
            }
            return typeof(KerykeionBaseClass).IsAssignableFrom(entityType.ClrType);
        }

        #region PrivateHelperMethods
        private async Task<bool> ExistsAsync(string name, string tableName)
        {
            if (!InheritsFromKeryKeionBaseClass(FindEntityTypeByTableName(tableName)))
            {
                return false;
            }
            var allEntities = await ListAllAsync(tableName);
            return allEntities.Select(o => o.GetType().GetProperty(PropertyNameConstants.UniqueNameIdentifier)?.GetValue(o)).Contains(name?.CompleteTrimAndUpper());
        }

        private async Task<KerykeionDbResult> SetPropertiesAsync(object entity, Dictionary<string, StringValues> formDict)
        {
            var entityType = FindEntityTypeByClrType(entity.GetType());
            List<KerykeionDbError> errors = new List<KerykeionDbError>();

            foreach (var prop in entityType.GetProperties())
            {
                var property = entityType.ClrType.GetProperty(prop.Name);
                if (property == null)
                {
                    if (formDict.ContainsKey(prop.Name))
                    {
                        var result = await SetForeignKeyAsync(entity, formDict[prop.Name].ToString(), prop.Name);

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
                                errors.Add(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.EntityNotExistsWithSpecifiedPriKey, $"There is no '{targetType?.Name}' found in the database with '{formDict[prop.Name]}' as primary key.", targetType?.Name, formDict[prop.Name]) });
                                continue;
                            }

                            var target = await Context.FindAsync(targetType, Guid.Parse(formDict[prop.Name]));
                            if (target == null)
                            {
                                errors.Add(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.EntityNotExistsWithSpecifiedPriKey, $"There is no '{targetType?.Name}' found in the database with '{formDict[prop.Name]}' as primary key.", targetType?.Name, formDict[prop.Name]) });
                                continue;
                            }

                            property.SetValue(entity, Guid.Parse(formDict[prop.Name]));
                            continue;
                        }

                        errors.Add(new KerykeionDbError
                        {
                            Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NotValidGuid, $"Please provide a valid GUID for the property '{prop.Name}'.", prop.Name)
                        });

                        continue;
                    }

                    if (prop.IsForeignKey())
                    {
                        var result = await SetForeignKeyAsync(entity, formDict[prop.Name].ToString(), prop.Name);

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

        private async Task<KerykeionDbResult> SetForeignKeyAsync(object entity, string formValue, string propertyName)
        {
            var foreignKey = FindEntityTypeByClrType(entity.GetType()).GetForeignKeys().FirstOrDefault(fk => fk.GetDefaultName().Contains(propertyName, StringComparison.OrdinalIgnoreCase));

            if (foreignKey == null) return null;

            if (string.IsNullOrEmpty(formValue?.Trim()))
            {
                if (foreignKey.IsRequired)
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.RequiredField, $"The field '{propertyName}' is required.", propertyName) });
                }

                Context.Entry(entity).Property(propertyName).CurrentValue = null;
                return null;
            }

            if (!Guid.TryParse(formValue, out _))
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.NotValidGuid, $"Please provide a valid GUID for the property '{propertyName}'.", propertyName) });
            }

            var foreignPrincipalEntityType = foreignKey?.PrincipalEntityType?.ClrType;
            if (foreignPrincipalEntityType == null) return null;

            var foreignEntity = await Context.FindAsync(foreignPrincipalEntityType, Guid.Parse(formValue));
            if (foreignEntity == null)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = _translationsService.TranslateErrorByDescriber(ErrorDescriberConstants.EntityNotExistsWithSpecifiedPriKey, $"There is no '{foreignPrincipalEntityType?.Name}' found in the database with '{formValue}' as primary key.", foreignPrincipalEntityType?.Name, formValue) });
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
        #endregion
    }
}
