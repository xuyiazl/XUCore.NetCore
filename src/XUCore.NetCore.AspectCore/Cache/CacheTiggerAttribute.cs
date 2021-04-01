using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CacheTiggerAttribute : InterceptorBase
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
        /// 刷新时间（秒）
        /// </summary>
        public int Seconds { get; set; } = 60;
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public bool IsOpen { get; set; } = true;
        /// <summary>
        /// 缓存拦截器
        /// </summary>
        public CacheTiggerAttribute() { }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (!IsOpen)
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
                var logger = context.ServiceProvider.GetService<ILogger<CacheTiggerAttribute>>();

                logger.LogError($"CacheInterceptor：Key：{Key}，ParamterKey：{ParamterKey} {ex.FormatMessage()}");

                await next(context);
            }
        }

        private async Task Next(AspectContext context, AspectDelegate next, ICacheService cacheService, string key)
        {
            var scheduler = context.ServiceProvider.GetService<QuartzService>();

            if (!scheduler.CacheContainer.ContainsKey(key))
            {
                scheduler.CacheContainer.TryAdd(key, async () =>
                {
                    await next(context);

                    var value = await context.GetReturnValue();

                    if (value != null)
                        cacheService.Set(key, value);
                });

                scheduler.JoinJobAsync(key, TimeSpan.FromSeconds(Seconds)).Wait();
            }

            await next(context);

            var value = await context.GetReturnValue();

            if (value != null)
                cacheService.Set(key, value);
        }

    }
}
