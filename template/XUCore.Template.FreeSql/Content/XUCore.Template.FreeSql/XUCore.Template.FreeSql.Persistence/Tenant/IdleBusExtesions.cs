using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.Persistence
{
    public static class IdleBusExtesions
    {
        /// <summary>
        /// 创建FreeSql实例（动态创建FreeSql实例），目前可能用到的是saas里的租户
        /// </summary>
        /// <param name="connectionSettings">系统主库配置</param>
        /// <param name="dataType">租户数据库类型</param>
        /// <param name="connectionString">租户数据库连接字符串</param>
        /// <returns></returns>
        private static IFreeSql CreateFreeSql(ConnectionSettings connectionSettings, DataType dataType, string connectionString)
        {
            var freeSqlBuilder = new FreeSqlBuilder()
                       .UseConnectionString(dataType, connectionString)
                       .UseAutoSyncStructure(false)
                       .UseLazyLoading(false)
                       .UseNoneCommandParameter(true);

            //监听所有命令
            if (connectionSettings.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }

            var fsql = freeSqlBuilder.Build();

            //全局过滤租户数据
            //fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //配置实体
            DbHelper.ConfigEntity(fsql);

            //监听Curd操作
            if (connectionSettings.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            //计算服务器时间
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset);
            };

            return fsql;
        }

        /// <summary>
        /// 获得FreeSql实例
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IFreeSql GetFreeSql(this IdleBus<IFreeSql> ib, IServiceProvider serviceProvider)
        {
            /*
             * 
             *  注释的代码为租户创建实例。该架构不需要使用（Saas需要）
             *  
            var user = serviceProvider.GetRequiredService<IUser>();
            var appConfig = serviceProvider.GetRequiredService<AppConfig>();

            if (appConfig.Tenant && user.DataIsolationType == DataIsolationType.OwnDb && user.TenantId.HasValue)
            {
                var tenantName = "tenant_" + user.TenantId.ToString();
                var exists = ib.Exists(tenantName);
                if (!exists)
                {
                    var connectionSettings = serviceProvider.GetRequiredService<ConnectionSettings>();

                    //查询租户数据库信息
                    var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
                    var tenantRepository = freeSql.GetRepository<TenantEntity>();
                    var tenant = tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(user.TenantId).ToOne<FreeSqlTenantDto>();
                    // 数据库类型
                    var dataType = DataType.MySql;
                    // 连接字符串
                    var connectionStrings = "";
                    // 空闲时间
                    var timeSpan = tenant.IdleTime.HasValue && tenant.IdleTime.Value > 0 ? TimeSpan.FromMinutes(tenant.IdleTime.Value) : TimeSpan.MaxValue;

                    ib.TryRegister(tenantName, () => CreateFreeSql(connectionSettings, dataType, connectionStrings), timeSpan);
                }

                return ib.Get(tenantName);
            }
            else
            {
                var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
                return freeSql;
            }
            */

            var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
            return freeSql;
        }
    }
}
