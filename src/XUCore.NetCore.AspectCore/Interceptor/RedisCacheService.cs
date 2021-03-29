using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using XUCore.Extensions;
using XUCore.NetCore.Redis;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    public class RedisCacheService : ICacheService
    {
        private readonly IRedisService redisService;
        private readonly ILogger<RedisCacheService> logger;

        public RedisCacheService(ILogger<RedisCacheService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.redisService = serviceProvider.GetService<IRedisService>();
        }

        public object Get(string key, Type returnType)
        {
            try
            {
                var json = redisService.StringGet<string>(key, connectionName: "cache-read", serializer: RedisSerializerOptions.RedisValue);

                return JsonConvert.DeserializeObject(json, returnType);
            }
            catch (Exception ex)
            {
                logger.LogError($"获取缓存失败，{ex.FormatMessage()}");
                return null;
            }
        }

        public void Set(string key, object value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);

                redisService.StringSet(key, json, connectionName: "cache-write", serializer: RedisSerializerOptions.RedisValue);
            }
            catch (Exception ex)
            {
                logger.LogError($"写入 {key} 缓存失败，{ex.FormatMessage()}");
            }
        }

        public void Set(string key, TimeSpan expirationTime, object value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);

                redisService.StringSet(key, json, (int)expirationTime.TotalSeconds, connectionName: "cache-write", serializer: RedisSerializerOptions.RedisValue);
            }
            catch (Exception ex)
            {
                logger.LogError($"写入 {key} 缓存失败，{ex.FormatMessage()}");
            }
        }

        public void Remove(string key)
        {
            redisService.KeyDelete(key, connectionName: "cache-write");
        }
    }
}
