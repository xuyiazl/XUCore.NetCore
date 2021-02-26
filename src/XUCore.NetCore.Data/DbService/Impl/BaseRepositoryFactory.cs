using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XUCore.NetCore.Data.BulkExtensions;

namespace XUCore.NetCore.Data.DbService
{
    public abstract class BaseRepositoryFactory : DBContextFactory
    {
        private readonly Assembly assembly;
        protected BaseRepositoryFactory(Type type, DbContextOptions options, DbServer dbServer, string mappingPath) 
            : base(options, mappingPath)
        {
            this.assembly = type.Assembly;

            foreach (var extensions in options.Extensions)
            {
                //MYSQL ==>>>>>  Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension
                //MSSQL ==>>>>>  Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension

                switch (dbServer)
                {
                    case DbServer.MySql:
                        if (extensions.GetType().FullName.Equals("Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension"))
                            this.ConnectionStrings = (extensions as MySqlOptionsExtension).ConnectionString;
                        break;
                    case DbServer.SqlServer:
                        if (extensions.GetType().FullName.Equals("Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension"))
                            this.ConnectionStrings = (extensions as SqlServerOptionsExtension).ConnectionString;
                        break;
                    default:
                        throw new Exception("暂时只支持mysql、sqlserver。");
                }
            }
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public string ConnectionStrings { get; set; }


        /// <summary>
        /// EF依赖mappingPath，将当前项目文件夹的Entity的映射文件执行注入操作
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //扫描指定文件夹的
            var typesToRegister = assembly.GetTypes()
           .Where(type => !string.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractEntityTypeConfiguration<>))
           .Where(type => type.Namespace.Contains(mappingPath));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
