using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IServiceProvider serviceProvider)
        {
            this.memoryCache = serviceProvider.GetService<IMemoryCache>();
        }

        public object Get(string key, Type returnType)
        {
            return memoryCache.Get(key);
        }

        public void Set(string key, object value)
        {
            memoryCache.Set(key, value);
        }

        public void Set(string key, TimeSpan expirationTime, object value)
        {
            memoryCache.Set(key, value, DateTimeOffset.Now.AddSeconds(expirationTime.TotalSeconds));
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
