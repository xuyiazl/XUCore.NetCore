﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using XUCore.Extensions;
using XUCore.NetCore.Redis;

namespace XUCore.NetCore.AspectCore.Cache
{
    internal class RedisCacheService : ICacheService
    {
        private readonly IRedisService redisService;
        private readonly ILogger<RedisCacheService> logger;
        private readonly IOptions<CacheOptions> option;

        public RedisCacheService(ILogger<RedisCacheService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.redisService = serviceProvider.GetService<IRedisService>();
            this.option = serviceProvider.GetService<IOptions<CacheOptions>>();
        }

        public object Get(string key, Type returnType)
        {
            try
            {
                var json = redisService.StringGet<string>(key, connectionName: option.Value.RedisRead, serializer: RedisSerializerOptions.RedisValue);

                if (json.IsEmpty())
                    return null;

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
                if (value.IsNull())
                    return;

                var json = JsonConvert.SerializeObject(value);

                redisService.StringSet(key, json, connectionName: option.Value.RedisWrite, serializer: RedisSerializerOptions.RedisValue);
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
                if (value.IsNull())
                    return;

                var json = JsonConvert.SerializeObject(value);

                redisService.StringSet(key, json, (int)expirationTime.TotalSeconds, connectionName: option.Value.RedisWrite, serializer: RedisSerializerOptions.RedisValue);
            }
            catch (Exception ex)
            {
                logger.LogError($"写入 {key} 缓存失败，{ex.FormatMessage()}");
            }
        }

        public void Remove(string key)
        {
            redisService.KeyDelete(key, connectionName: option.Value.RedisWrite);
        }
    }
}
