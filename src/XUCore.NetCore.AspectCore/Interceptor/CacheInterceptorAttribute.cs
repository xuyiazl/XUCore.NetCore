using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public string Prefix { get; set; } = "_cachePrefix_";
        /// <summary>
        /// 缓存key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 缓存key（参数组成部分，如果不需要则不填写），参考string.Format的参数顺序
        /// </summary>
        public string ParamterKey { get; set; }
        /// <summary>
        /// 缓存时间（秒）
        /// </summary>
        public int CacheSeconds { get; set; } = 0;
        /// <summary>
        /// 是否开启后台触发同步缓存（注意：由于资源被回收或者被释放的问题，使用同步必须是单例模式）
        /// </summary>
        public bool IsTigger { get; set; } = false;
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public bool IsOpen { get; set; } = true;
        /// <summary>
        /// 缓存拦截器
        /// </summary>
        public CacheInterceptorAttribute() { }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (!IsOpen || CacheSeconds <= 0)
            {
                await next(context);

                return;
            }

            try
            {
                var cacheService = context.ServiceProvider.GetService<ICacheService>();

                Type returnType = context.GetReturnType();

                string key = Utils.GetParamterKey(Prefix, Key, ParamterKey, context.Parameters);

                var result = cacheService.Get(key, returnType);

                if (result == null)
                {
                    await Next(context, next, cacheService, key);
                }
                else
                {
                    context.ReturnValue = context.ResultFactory(result, returnType, context.IsAsync());
                }
            }
            catch (Exception ex)
            {
                var logger = context.ServiceProvider.GetService<ILogger<CacheInterceptorAttribute>>();

                logger.LogError($"CacheInterceptor：Key：{Key}，ParamterKey：{ParamterKey} {ex.FormatMessage()}");

                await next(context);
            }
        }

        private async Task Next(AspectContext context, AspectDelegate next, ICacheService cacheService, string key)
        {
            if (IsTigger)
            {
                var scheduler = context.ServiceProvider.GetService<QuartzService>();

                if (!scheduler.CacheContainer.ContainsKey(key))
                {
                    scheduler.CacheContainer.TryAdd(key, () =>
                    {
                        next(context).Wait();

                        var value = context.GetReturnValue().Result;

                        if (value != null)
                            cacheService.Set(key, value);
                    });

                    scheduler.JoinJobAsync(key, TimeSpan.FromSeconds(CacheSeconds)).Wait();
                }

                await next(context);

                var value = await context.GetReturnValue();

                if (value != null)
                    cacheService.Set(key, value);
            }
            else
            {
                await next(context);

                var value = await context.GetReturnValue();

                if (value != null)
                    cacheService.Set(key, TimeSpan.FromSeconds(CacheSeconds), value);
            }
        }

    }
}
