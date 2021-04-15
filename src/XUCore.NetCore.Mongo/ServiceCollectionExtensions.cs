using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Mongo注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddMongoService(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.TryAddSingleton(typeof(IMongoRepository), typeof(MongoRepository));

            return services;
        }
    }
}
