using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.Redis;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class RedisCacheMethodAttribute : InterceptorBase
    {
        /// <summary>
        /// HashKey
        /// </summary>
        public string HashKey { get; set; }
        /// <summary>
        /// 缓存key（参数组成部分，如果不需要则不填写），参考string.Format的参数顺序。
        /// 支持属性参数的替换，{Id}-{Name} 等于 1-test
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 缓存时间（秒）
        /// </summary>
        public int Seconds { get; set; }
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public bool IsOpen { get; set; } = true;
        /// <summary>
        /// 缓存拦截器
        /// </summary>
        public RedisCacheMethodAttribute() { }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (!IsOpen)
            {
                await next(context);

                return;
            }

            try
            {
                var redis = context.ServiceProvider.GetService<IRedisService>();
                var option = context.ServiceProvider.GetService<IOptions<CacheOptions>>();

                Type returnType = context.GetReturnType();

                string key = Utils.GetParamterKey("", "", Key, context.Parameters);

                var result = redis.HashGet<string>(HashKey, key, option.Value.RedisRead, RedisSerializerOptions.RedisValue);

                if (result.IsEmpty())
                {
                    await next(context);

                    var value = await context.GetReturnValue();

                    if (value != null)
                    {
                        var json = JsonConvert.SerializeObject(value);

                        if (Seconds > 0)
                        {
                            bool exists = redis.KeyExists(HashKey, option.Value.RedisRead);

                            redis.HashSet(HashKey, key, json, option.Value.RedisWrite, RedisSerializerOptions.RedisValue);

                            if (!exists)
                                redis.KeyExpire(HashKey, Seconds, option.Value.RedisWrite);
                        }
                        else
                            redis.HashSet(HashKey, key, json, option.Value.RedisWrite, RedisSerializerOptions.RedisValue);
                    }
                }
                else
                {
                    var value = JsonConvert.DeserializeObject(result, returnType);

                    context.ReturnValue = context.ResultFactory(value, returnType, context.IsAsync());
                }
            }
            catch (Exception ex)
            {
                var logger = context.ServiceProvider.GetService<ILogger<RedisCacheMethodAttribute>>();

                logger.LogError($"RedisCacheMethod：Key：{Key} {ex.FormatMessage()}");

                await next(context);
            }
        }
    }
}
