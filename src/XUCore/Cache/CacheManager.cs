using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Cache
{
    /// <summary>
    /// 本地缓存管理
    /// </summary>
    public class CacheManager : ICacheManager
    {
        /// <summary>
        /// MemoryCache缓存接口
        /// </summary>
        protected IMemoryCache _cache;
        public CacheManager(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        /// <summary>
        /// 是否存在此缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue<object>(key, out _);
        }
        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T v))
                return v;
            else
                return default;
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.Run(() => Get<T>(key));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.Get(key);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key)
        {
            return await Task.Run(() => Get(key));
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));

            return dict;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _cache.GetOrCreate(key, factory);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> factory)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _cache.GetOrCreateAsync(key, factory);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Set(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (_cache.TryGetValue(key, out _))
                _cache.Remove(key);
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, object value)
        {
            return await Task.Run(() => Set(key, value));
        }


        /// <summary>
        /// 设置缓存,绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationMinute">间隔分钟</param>
        public void Set(string key, object value, double expirationMinute)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (_cache.TryGetValue(key, out _))
                _cache.Remove(key);
            DateTime now = DateTime.Now;
            TimeSpan ts = now.AddMinutes(expirationMinute) - DateTime.Now;
            _cache.Set(key, value, ts);
        }

        /// <summary>
        /// 设置缓存,绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime">DateTimeOffset 结束时间</param>
        public void Set(string key, object value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (_cache.TryGetValue(key, out _))
                _cache.Remove(key);

            _cache.Set(key, value, expirationTime);
        }

        /// <summary>
        /// 设置缓存,相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        public void SetSliding(string key, object value, TimeSpan t)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (_cache.TryGetValue(key, out _))
                _cache.Remove(key);

            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = t
            });
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Set(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
                    );

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="onUpdateCallback">从缓存中删除缓存条目后，将触发给定的回调</param>
        /// <returns></returns>
        public bool SetCallback(string key, object value, PostEvictionDelegate onUpdateCallback)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }


            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .RegisterPostEvictionCallback(onUpdateCallback));

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <param name="onUpdateCallback">从缓存中删除缓存条目后，将触发给定的回调</param>
        /// <returns></returns>
        public bool SetAbsoulteCallback(string key, object value, TimeSpan expiressAbsoulte, PostEvictionDelegate onUpdateCallback)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }


            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiressAbsoulte)
                    .RegisterPostEvictionCallback(onUpdateCallback));

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="onUpdateCallback">从缓存中删除缓存条目后，将触发给定的回调</param>
        /// <returns></returns>
        public bool SetSlidingCallback(string key, object value, TimeSpan expiresSliding, PostEvictionDelegate onUpdateCallback)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }


            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .RegisterPostEvictionCallback(onUpdateCallback));

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <param name="onUpdateCallback">从缓存中删除缓存条目后，将触发给定的回调</param>
        /// <returns></returns>
        public bool SetCallback(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte, PostEvictionDelegate onUpdateCallback)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }


            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
                    .RegisterPostEvictionCallback(onUpdateCallback));

            return Exists(key);
        }


        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
                if (!Remove(key)) return false;

            return Set(key, value);

        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Exists(key))
                if (!Remove(key)) return false;

            return Set(key, value, expiresSliding, expiressAbsoulte);
        }


        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _cache.Remove(key);

            return !Exists(key);
        }
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            keys.ToList().ForEach(item => _cache.Remove(item));
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
        }


    }
}
