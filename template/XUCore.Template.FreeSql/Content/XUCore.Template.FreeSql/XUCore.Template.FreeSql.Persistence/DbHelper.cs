using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.FreeSql;
using XUCore.NetCore.FreeSql.Entity;
using XUCore.Template.FreeSql.Core;

namespace XUCore.Template.FreeSql.Persistence
{
    public class DbHelper
    {
        /// <summary>
        /// 偏移时间
        /// </summary>
        public static TimeSpan TimeOffset;

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabaseAsync(ConnectionSettings dbConfig)
        {
            if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                    .Build();

            try
            {
                Console.WriteLine("\r\n create database started");
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine(" create database succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine($" create database failed.\n {e.Message}");
            }
        }

        /// <summary>
        /// 获得指定程序集表实体
        /// </summary>
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            List<string> assemblyNames = new List<string>()
            {
                "XUCore.Template.FreeSql.Persistence"
            };

            List<Type> entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
                {
                    foreach (Attribute attribute in type.GetCustomAttributes())
                    {
                        if (attribute is TableAttribute tableAttribute)
                        {
                            if (tableAttribute.DisableSyncStructure == false)
                            {
                                entityTypes.Add(type);
                            }
                        }
                    }
                }
            }

            return entityTypes.ToArray();
        }

        /// <summary>
        /// 配置实体
        /// </summary>
        public static void ConfigEntity(IFreeSql db)
        {
            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                db.CodeFirst.Entity(entityType, a =>
                {
                    //a.Ignore(tenantId);
                });
            }
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="timeOffset"></param>
        /// <param name="user"></param>
        public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUserInfo user)
        {
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            && snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = Id.SnowflakeId;
            }


            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case nameof(EntityAdd.CreatedAtUserId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user?.Id;
                        }
                        break;

                    case nameof(EntityAdd.CreatedAtUserName):
                        if (e.Value == null || e.Value.IsNull())
                        {
                            e.Value = user?.UserName;
                        }
                        break;

                    case nameof(EntityAdd.CreatedAt):
                        if (e.Value == null || e.Value.IsNull())
                        {
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case nameof(EntityUpdate.ModifiedAtUserId):
                        e.Value = user?.Id;
                        break;

                    case nameof(EntityUpdate.ModifiedAtUserName):
                        e.Value = user?.UserName;
                        break;

                    case nameof(EntityUpdate.ModifiedAt):
                        if (e.Value == null || e.Value.IsNull())
                        {
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, ConnectionSettings dbConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
            //    }
            //};

            // 同步结构
            var dbType = dbConfig.Type.ToString();

            Console.WriteLine($"\r\n {(msg.NotEmpty() ? msg : $"sync {dbType} structure")} started");

            if (dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);

            Console.WriteLine($" {(msg.NotEmpty() ? msg : $"sync {dbType} structure")} succeed");
        }
    }
}
