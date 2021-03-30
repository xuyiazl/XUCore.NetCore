using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    /// <summary>
    /// 缓存拦截器（删除缓存）
    /// </summary>
    public class CacheRemoveAttribute : AbstractInterceptorAttribute
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
        /// 缓存key（参数组成部分，如果不需要则不填写），参考string.Format的参数顺序，（不支持模型）
        /// </summary>
        public string ParamterKey { get; set; }
        /// <summary>
        /// 缓存拦截器
        /// </summary>
        public CacheRemoveAttribute() { }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next(context);

            var cacheService = context.ServiceProvider.GetService<ICacheService>();

            string key = Utils.GetParamterKey(Prefix, Key, ParamterKey, context.Parameters);

            cacheService.Remove(key);
        }
    }
}
