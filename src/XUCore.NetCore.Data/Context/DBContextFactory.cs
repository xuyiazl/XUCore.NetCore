using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using XUCore.Extensions;

namespace XUCore.NetCore.Data
{
    public abstract class DBContextFactory : DBContextBase, IDbContext
    {
        protected DBContextFactory(DbContextOptions options) : base(options)
        {
            foreach (var extensions in options.Extensions)
            {
                //MYSQL ==>>>>>  Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension
                //MSSQL ==>>>>>  Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension

                var sqlOptions = extensions as RelationalOptionsExtension;

                if (sqlOptions != null)
                {
                    ConnectionStrings = sqlOptions.ConnectionString;
                    break;
                }

                //switch (dbServer)
                //{
                //    case DbServer.MySql:
                //        if (extensions.GetType().FullName.Equals("Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension"))
                //            this.ConnectionStrings = (extensions as MySqlOptionsExtension).ConnectionString;
                //        break;
                //    case DbServer.SqlServer:
                //        if (extensions.GetType().FullName.Equals("Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension"))
                //            this.ConnectionStrings = (extensions as SqlServerOptionsExtension).ConnectionString;
                //        break;
                //    default:
                //        throw new Exception("暂时只支持mysql、sqlserver。");
                //}
            }
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// 需要检索的程序集
        /// </summary>
        public virtual Assembly[] Assemblies =>
            AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                !assembly.FullName.StartsWith("System") &&
                !assembly.FullName.StartsWith("Microsoft") &&
                !assembly.FullName.StartsWith("netstandard") &&
                !assembly.FullName.StartsWith("Pomelo")
            ).ToArray();

        /// <summary>
        /// EF依赖mappingPath，将当前项目文件夹的Entity的映射文件执行注入操作
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assemblies.GetTypes(type => type.IsAbstract == false && type.AnyBaseType(typeof(EntityTypeConfiguration<>)));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
