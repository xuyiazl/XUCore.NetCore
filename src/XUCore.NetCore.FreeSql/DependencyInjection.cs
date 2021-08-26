using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.FreeSql.Curd;

namespace XUCore.NetCore.FreeSql
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 注册工作单元管理
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSqlUnitOfWorkManager(this IServiceCollection services)
        {
            //services.AddScoped<IdleBusUnitOfWorkManager>();
            services.AddScoped<FreeSqlUnitOfWorkManager>();
            services.AddScoped(typeof(MarkUnitOfWorkManager<>));

            return services;
        }
    }
}
