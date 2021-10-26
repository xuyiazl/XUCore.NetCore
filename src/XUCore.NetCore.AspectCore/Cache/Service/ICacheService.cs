using System;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    internal interface ICacheService
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
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}