using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using XUCore.NetCore.Data.DbService;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    public abstract class BaseRepositoryFactory : DBContextFactory
    {
        protected BaseRepositoryFactory(DbContextOptions options, string dbType, string mappingPath) : base(options, mappingPath)
        {
            foreach (var extensions in options.Extensions)
            {
                //MYSQL ==>>>>>  Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension
                //MSSQL ==>>>>>  Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension
                
                switch (dbType.ToLower())
                {
                    case "mysql":
                        if (extensions.GetType().FullName.Equals("Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal.MySqlOptionsExtension"))
                            this.ConnectionStrings = (extensions as MySqlOptionsExtension).ConnectionString;
                        break;
                    case "mssql":
                    case "sqlserver":
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

    }
}
