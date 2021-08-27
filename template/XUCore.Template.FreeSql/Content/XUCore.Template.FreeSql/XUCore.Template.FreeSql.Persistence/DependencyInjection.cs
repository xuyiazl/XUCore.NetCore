using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using XUCore.Configs;
using XUCore.Ddd.Domain;
using XUCore.Extensions;
using XUCore.NetCore.FreeSql;
using XUCore.NetCore.FreeSql.Entity;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IdleBusUnitOfWorkManager>();
            services.AddFreeSqlUnitOfWorkManager();

            var connection = configuration.GetSection<ConnectionSettings>("ConnectionSettings");

            //创建数据库
            if (connection.CreateDb)
                DbHelper.CreateDatabaseAsync(connection).Wait();

            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(connection.Type, connection.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            //监听所有命令
            if (connection.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }

            var fsql = freeSqlBuilder.Build();

            //监听Curd操作
            if (connection.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            //全局过滤
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //配置实体

            DbHelper.ConfigEntity(fsql);

            //同步结构
            if (connection.SyncStructure)
                DbHelper.SyncStructure(fsql, dbConfig: connection);

            //计算服务器时间（时间偏移）
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            DbHelper.TimeOffset = timeOffset;

            var user = services.BuildServiceProvider().GetService<IUserInfo>();

            //审计数据
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
            };

            services.AddSingleton(new AspectCoreFreeSql { Orm = fsql });

            //导入多数据库
            if (!connection.Dbs.IsNull())
            {
                foreach (var multiDb in connection.Dbs)
                {
                    switch (multiDb.Name)
                    {
                        case nameof(MySqlDb):
                            services.AddSingleton(new AspectCoreFreeSql { Orm = CreateMultiDbBuilder(multiDb).Build<MySqlDb>() });
                            break;
                        default:
                            break;
                    }
                }
            }

            //添加IdleBus单例（管理租户用）
            //services.AddSingleton(new IdleBus<IFreeSql>(connection.IdleTime > 0 ? TimeSpan.FromMinutes(connection.IdleTime) : TimeSpan.MaxValue));

            return services;
        }


        /// <summary>
        /// 创建多数据库构建器
        /// </summary>
        /// <param name="multiDb"></param>
        /// <returns></returns>
        private static FreeSqlBuilder CreateMultiDbBuilder(MultiDb multiDb) =>
            new FreeSqlBuilder()
                    .UseConnectionString(multiDb.Type, multiDb.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);
    }
}