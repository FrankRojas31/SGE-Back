using Base.Domain.Entidades.Core;
using Base.Infraestructura.Datos.ContextoBD;
using Dapper;
using ExpressionExtensionSQL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace Base.Infraestructura.Data.Repositories.Implementation
{
    public class BaseRepository<T> where T : class
    {
        /// <summary>
        /// The context
        /// </summary>
        protected readonly DataBaseContext Context;
        /// <summary>
        /// The table name
        /// </summary>
        private string tableName;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>

        private readonly ClaimsPrincipal _user;
        public BaseRepository(DataBaseContext context, ClaimsPrincipal user)
        {
            Context = context;
            var model = context.Model;  

            // Get all the entity types information contained in the DbContext class, ...
            var entityTypes = model.GetEntityTypes();

            // ... and get one by entity type information of "FooBars" DbSet property.
            try
            {
                var entityTypeOfFooBar = entityTypes.First(t => t.ClrType == typeof(T));
                var tableNameAnnotation = entityTypeOfFooBar.GetAnnotation("Relational:TableName");
                var tableNameOfFooBarSet = tableNameAnnotation.Value.ToString();
                tableName = tableNameOfFooBarSet;
            } 
            catch (Exception ex)
            {
                //Una entidad que hereda de base entity que no tiene tabla
            }
            _user = user;
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual async Task<int> InsertAsync(T entity)
        {
            var properties = GetProperties(entity);
            var propertiesWithoutId = properties.Where(p => p.Name != "Id" && !p.GetAccessors().Any(x => x.IsVirtual)); // Excluye la propiedad "Id"
            var columnNames = string.Join(", ", propertiesWithoutId.Select(p => $"[{p.Name}]"));
            var parameterNames = string.Join(", ", propertiesWithoutId.Select(p => $"@{p.Name}"));
            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames}); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var idRow = await Context.Database.GetDbConnection().QuerySingleAsync<int>(sql, entity);

            AuditChange audit = new AuditChange()
            {
                Action = "INSERT",
                TableName = tableName,
                OldValue = "",
                NewValue = JsonConvert.SerializeObject(entity),
                User = this.ObtenerIdUsario(),
                Role = this.ObtenerRol(),
                IPAddress = "",
                RowVersion = DateTime.Now,
                EsBorrado = false,
                IdEntity = idRow
            };

            var result = await AuditTable(audit);

            return idRow;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T entity, T entityOld = null)
        {

            var properties = GetProperties(entity);
            var propertiesWithoutId = properties.Where(p => p.Name != "Id" && !p.GetAccessors().Any(x => x.IsVirtual)); // Excluye la propiedad "Id"
            var updateColumns = propertiesWithoutId.Select(p => $"[{p.Name}] = @{p.Name}");
            var sql = $"UPDATE {tableName} SET {string.Join(", ", updateColumns)} WHERE Id = @Id";
            var result = await Context.Database.GetDbConnection().ExecuteAsync(sql, entity);

            if (result != 0 && entityOld != null)
            {
                AuditChange audit = new AuditChange()
                {
                    Action = "UPDATE",
                    TableName = tableName,
                    OldValue = JsonConvert.SerializeObject(entityOld),
                    NewValue = JsonConvert.SerializeObject(entity),
                    User = this.ObtenerIdUsario(),
                    Role = this.ObtenerRol(),
                    IPAddress = "",
                    RowVersion = DateTime.Now,
                    EsBorrado = false,
                    IdEntity = (int)entityOld.GetType().GetProperty("Id").GetValue(entity)
                };

                var resultAudit = await AuditTable(audit);
            }

            return result;
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(T entity)
        {
            var sql = $"UPDATE {tableName} SET EsBorrado = 1 WHERE Id = @Id";


            var result = await Context.Database.GetDbConnection().ExecuteAsync(sql, entity);
            if (result != 0)
            {
                AuditChange audit = new AuditChange()
                {
                    Action = "DELETE",
                    TableName = tableName,
                    OldValue = JsonConvert.SerializeObject(entity),
                    NewValue = string.Empty,
                    User = this.ObtenerIdUsario(),
                    Role = this.ObtenerRol(),
                    IPAddress = "",
                    RowVersion = DateTime.Now,
                    EsBorrado = false,
                    IdEntity = (int)entity.GetType().GetProperty("Id").GetValue(entity)
                };

                var resultAudit = await AuditTable(audit);
            }

            return result;
        }

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<int> RemoveAsync(int id)
        {

            var sql = $"UPDATE {tableName} SET EsBorrado  = 1 WHERE Id = @Id";
           
            var result = await Context.Database.GetDbConnection().ExecuteAsync(sql, new { id });

            if (result != 0)
            {
                AuditChange audit = new AuditChange()
                {
                    Action = "DELETE",
                    TableName = tableName,
                    OldValue = string.Empty,
                    NewValue = string.Empty,
                    User = this.ObtenerIdUsario(),
                    Role = this.ObtenerRol(),
                    IPAddress = "",
                    RowVersion = DateTime.Now,
                    EsBorrado = false,
                    IdEntity = id
                };

                var resultAudit = await AuditTable(audit);
            }

            return result;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetProperties(T entity)
        {
            return entity.GetType()
                .GetProperties()
                .Where(p => p.CanWrite);
        }

        private string GenerateSQL(ref DynamicParameters parameters, Expression<Func<T, bool>>? filter = null, string? fields = null, string? aditionalParameters = null)
        {
            var isDeletable = true;
            var sql = $"SELECT * FROM {tableName}";
            if (parameters == null) parameters = new DynamicParameters();
            if (filter is null)
            {
                if (isDeletable)
                {
                    sql += " WHERE EsBorrado = 0";
                }

                if (!string.IsNullOrEmpty(aditionalParameters))
                {
                    sql += aditionalParameters;

                }
            }
            else
            {
                var queryGenerated = filter.ToSql();// aqui se utiliza la libreria ExpressionExtensionSQL que es un conversor de lambda expresion to Sql
                string filterSql = queryGenerated.Sql;
                var className = typeof(T).Name;
                filterSql = filterSql.Replace(className, tableName);
                if (fields != null)
                {
                    sql = $"SELECT " + fields + " FROM {tableName} WHERE {filterSql}";
                }
                else
                {
                    sql = $"SELECT * FROM {tableName} WHERE {filterSql}";
                }

                if (isDeletable)
                {
                    sql += " AND EsBorrado = 0";
                }

                if (!string.IsNullOrEmpty(aditionalParameters))
                {
                    sql += aditionalParameters;

                }
                foreach (var item in queryGenerated.Parameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            return sql;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            var parameters = new DynamicParameters();
            var sql = this.GenerateSQL(ref parameters, filter, null);

            return (await Context.Database.GetDbConnection().QueryAsync<T>(sql, parameters)).ToList();
        }

        /// <summary>
        /// Gets the single asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual async Task<T?> GetSingleAsync(Expression<Func<T, bool>>? filter = null)
        {
            var parameters = new DynamicParameters();
            var sql = this.GenerateSQL(ref parameters, filter);

            return await Context.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public virtual async Task<bool> ExistSingleAsync(Expression<Func<T, bool>>? filter = null)
        {
            return (await GetSingleAsync(filter) != null);
        }

        public virtual async Task<T?> GetById(int Id)
        {
            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
            var result = await Context.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(sql, new { Id = Id });
            return result;
        }

        /// <summary>
        /// Gets the comparisonl operation.
        /// </summary>
        /// <param name="nodeType">Type of the node.</param>
        /// <returns></returns>
        private string GetComparisonlOperation(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                default:
                    return null;
            }
        }

        public async Task<int> CountRow(string filter)
        {
            var sql = $"SELECT count(*) FROM {tableName}";

            if (!string.IsNullOrEmpty(filter))
            {
                sql += " WHERE EsBorrado = 0 ";
                sql += $" {filter}";
            }

            var result = await Context.Database.GetDbConnection().QueryAsync<int>(sql);
            return result.FirstOrDefault();
        }

        public string ObtenerIdUsario()
        {
            if (this._user == null || !this._user.Identity.IsAuthenticated)
            {
                return string.Empty;
            }
            return this._user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public string ObtenerRol()
        {
            if (this._user == null || !this._user.Identity.IsAuthenticated)
            {
                return string.Empty;
            }
            return this._user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }

        public virtual async Task<int> InsertRangeAsync(IEnumerable<T> entities)
        {
            var connection = Context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var entity in entities)
                {
                    var properties = GetProperties(entity);
                    var propertiesWithoutId = properties
                        .Where(p => p.Name != "Id" && !p.GetAccessors().Any(x => x.IsVirtual)); // Excluye la propiedad "Id"

                    var columnNames = string.Join(", ", propertiesWithoutId.Select(p => $"[{p.Name}]"));
                    var parameterNames = string.Join(", ", propertiesWithoutId.Select(p => $"@{p.Name}"));

                    var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames}); SELECT CAST(SCOPE_IDENTITY() AS INT)";

                    await connection.ExecuteAsync(sql, entity, transaction: transaction);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }

            return entities.Count();
        }

        public async Task<int> AuditTable(AuditChange audit)
        {
            var sql = @"INSERT INTO [dbo].[Tbl_AuditChanges]
                                   ([TableName]
                                   ,[OldValue]
                                   ,[NewValue]
                                   ,[User]
                                   ,[Role]
                                   ,[IPAddress]
                                   ,[RowVersion]
                                   ,[EsBorrado]
                                   ,[IdEntity]
                                   ,[Action])
                             VALUES
                                   (@TableName
                                   ,@OldValue
                                   ,@NewValue
                                   ,@User
                                   ,@Role
                                   ,@IPAddress
                                   ,@RowVersion
                                   ,@EsBorrado
                                   ,@IdEntity
                                   ,@Action)";


            var result = await Context.Database.GetDbConnection().QueryAsync<int>(sql, audit);

            return result.FirstOrDefault();
        }
    }
}
