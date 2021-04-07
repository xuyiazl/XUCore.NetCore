using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using XUCore.NetCore.Redis;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// 缓存拦截器（删除缓存）
    /// </summary>
    public class RedisCacheRemoveAttribute : InterceptorBase
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
        /// 缓存拦截器
        /// </summary>
        public RedisCacheRemoveAttribute() { }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next(context);

            var redis = context.ServiceProvider.GetService<IRedisService>();
            var option = context.ServiceProvider.GetService<IOptions<CacheOptions>>();

            string key = Utils.GetParamterKey("", "", Key, context.Parameters);

            redis.HashDelete(HashKey, key, option.Value.RedisWrite);
        }
    }
}
