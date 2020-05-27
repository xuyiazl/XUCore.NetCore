using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Redis 列表命令
    /// </summary>
    public interface IListRedisCommand
    {
        /// <summary>
        /// 从列表左侧插入(只有当队列存在的时候)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListLeftPushWhenExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 从列表左侧插入(插入多数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListLeftPush<T>(string key, List<T> value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 从列表左侧插入(只有当队列不存在的时候)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListLeftPushWhenNoExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 从列表右侧插入(只有当列表存在时)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListRightPushWhenExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 从列表右侧插入(只有当列表不存在时)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListRightPushWhenNoExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 从列表右侧插入(插入多数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListRightPush<T>(string key, List<T> value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 返回最前面的一条并且移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        T ListLeftPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 返回最后面的一条并且移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        T ListRightPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得消息列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        long ListLength(string key, string connectionName = null);
        /// <summary>
        /// 通过下标设置消息列表值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index">允许负数(如果为负数则从最后开始算起)</param>
        /// <param name="value"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        void ListSetByIndex<T>(string key, long index, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 对消息列表进行截取操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="end"></param>
        /// <param name="connectionName"></param>
        void ListTrim(string key, long index, long end, string connectionName = null);
        /// <summary>
        /// 通过下标获得值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        T ListGetByIndex<T>(string key, long index, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 通过下标范围取的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        IList<T> ListRange<T>(string key, long start, long end, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 在消息列表指定值之前插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="insertvalue"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        long ListInsertBefore<T>(string key, T value, string insertvalue, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 在消息列表指定值之后插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="insertvalue"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        long ListInsertAfter<T>(string key, T value, string insertvalue, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 移除消息列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="removecount">0:移除所有相同的,大于0从左边开始移除相同的个数,小于0从末尾开始移除的个数</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        long ListRemove<T>(string key, T value, long removecount, string connectionName = null, IRedisSerializer serializer = null);
    }
}
