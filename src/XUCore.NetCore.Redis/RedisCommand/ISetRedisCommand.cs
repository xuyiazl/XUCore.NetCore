using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Redis Set集合
    /// </summary>
    public interface ISetRedisCommand
    {
        /// <summary>
        /// 集合添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">集合key</param>
        /// <param name="value">集合值</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>T
        bool SetAdd<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得集合里面的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">集合key</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        IList<T> SetMembers<T>(string key, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 查看集合里面是否有该内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">集合key</param>
        /// <param name="value">集合值</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        bool SetExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 移除集合中指定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">集合key</param>
        /// <param name="value">集合值</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        bool SetRemove<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 随机移除集合中的一个元素并且返回该值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        T SetPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 返回Set集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        long SetLength(string key, string connectionName = null);
        /// <summary>
        /// 随机返回集合中的元素 但是不删除,区别于POP操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        T SetRandom<T>(string key, string connectionName = null, IRedisSerializer serializer = null);
    }
}
