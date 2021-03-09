using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Redis注入
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddRedisService(this IServiceCollection services)
        {
            services.TryAddSingleton<IRedisService, RedisServiceProvider>();

            return services;
        }
        /// <summary>
        /// Redis注入序列化组件
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="redisSerializer">序列化</param>
        public static IServiceCollection AddRedisSerializer(this IServiceCollection services, IRedisSerializer redisSerializer)
        {
            services.TryAddSingleton<IRedisSerializer>(redisSerializer);

            return services;
        }
        /// <summary>
        /// Redis注入原生态序列化组件不改变任何结构类型
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddRedisValueRedisSerializer(this IServiceCollection services)
        {
            services.TryAddSingleton<IRedisSerializer, RedisValueSerializer>();

            return services;
        }
        /// <summary>
        /// Redis注入Json序列化组件
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="serializerSettings">JSON序列化配置</param>
        public static IServiceCollection AddJsonRedisSerializer(this IServiceCollection services, JsonSerializerSettings serializerSettings = null)
        {
            services.TryAddSingleton<IRedisSerializer>(new JsonRedisSerializer(serializerSettings));

            return services;
        }
        /// <summary>
        /// Redis注入MessagePack序列化组件
        /// </summary>
        /// <param name="services">服务集合</param>
        public static IServiceCollection AddMessagePackRedisSerializer(this IServiceCollection services)
        {
            services.TryAddSingleton<IRedisSerializer, MessagePackRedisSerializer>();

            return services;
        }
    }
}
