using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Cache
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
            if (value.IsNull())
                return;

            memoryCache.Set(key, value);
        }

        public void Set(string key, TimeSpan expirationTime, object value)
        {
            if (value.IsNull())
                return;

            memoryCache.Set(key, value, DateTimeOffset.Now.AddSeconds(expirationTime.TotalSeconds));
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
