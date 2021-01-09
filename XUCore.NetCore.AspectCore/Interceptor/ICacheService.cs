using System;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        object Get(string key, Type returnType);
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirationTime"></param>
        /// <param name="value"></param>
        void Set(string key, TimeSpan expirationTime, object value);
    }
}