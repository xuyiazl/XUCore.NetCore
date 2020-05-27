using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Hash 命令
    /// </summary>
    public interface IHashRedisCommand
    {
        /// <summary>
        /// 写入hash表操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId">表ID(可以当成是一行数据）</param>
        /// <param name="key">字段名（可以当成是列名）</param>
        /// <param name="value">字段值（可以当成是列值）</param>
        /// <param name="connectionName">连接名称</param>
        /// <param name="serializer">序列化</param>
        bool HashSet<T>(string hashId, string key, T value, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 写入时候判断是否写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isAlways">0=一直,1=仅存在的时候,2=不存在的时候</param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        bool HashSet<T>(string hashId, string key, T value, OverWrittenTypeDenum isAlways, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得Hash键值对值（可以理解为获得某一行中的某一列数据），当数据为空 则重新写入redis数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hashKey"></param>
        /// <param name="key"></param>
        /// <param name="fetcher"></param>
        /// <param name="seconds"></param>
        /// <param name="connectionRead"></param>
        /// <param name="connectionWrite"></param>
        /// <param name="isCache">是否缓存，默认true，如果设置为false则不走缓存直接走fetcher</param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        TResult HashGetOrInsert<TResult>(string hashKey, string key, Func<TResult> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null, bool isCache = true, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得Hash键值对值（可以理解为获得某一行中的某一列数据），当数据为空 则重新写入redis数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hashKey"></param>
        /// <param name="key"></param>
        /// <param name="fetcher"></param>
        /// <param name="t"></param>
        /// <param name="seconds"></param>
        /// <param name="connectionRead"></param>
        /// <param name="connectionWrite"></param>
        /// <param name="isCache">是否缓存，默认true，如果设置为false则不走缓存直接走fetcher</param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        TResult HashGetOrInsert<T, TResult>(string hashKey, string key, Func<T, TResult> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null, bool isCache = true, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得Hash键值对值（可以理解为获得某一行中的某一列数据）
        /// </summary>
        /// <param name="hashId">键ID（行ID）</param>
        /// <param name="key">键（列名）</param>
        /// <param name="connectionName">连接名称</param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        TResult HashGet<TResult>(string hashId, string key, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得Hash指定键值
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keys"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        IList<TResult> HashGet<TResult>(string hashId, string[] keys, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 获得Hash长度
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        long HashLength(string hashId, string connectionName = null);
        /// <summary>
        /// 获得Hash指定行的值
        /// </summary>
        /// <param name="hashId">键Id(行ID）</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        Dictionary<string, string> HashGetAll(string hashId, string connectionName = null);
        /// <summary>
        /// 获得Hash指定行的值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="connectionName"></param>
        /// <param name="fetcher"></param>
        /// <returns></returns>
        TResult HashGetAll<TResult>(string hashId, Func<Dictionary<string, string>, TResult> fetcher, string connectionName = null);
        /// <summary>
        /// 获得指定Hash所有的KEYS
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IList<string> HashKeys(string hashId, string connectionName = null);
        /// <summary>
        /// 获得指定Hash所有的Values
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hashId"></param>
        /// <param name="connectionName"></param>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        IList<TResult> HashValues<TResult>(string hashId, string connectionName = null, IRedisSerializer serializer = null);
        /// <summary>
        /// 删除Hash中指定的key
        /// </summary>
        /// <param name="hashId">键Id(行ID）</param>
        /// <param name="Key">键（列名）</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        bool HashDelete(string hashId, string Key, string connectionName = null);
        /// <summary>
        /// 删除Hash中指定的key
        /// </summary>
        /// <param name="hashId">键Id(行ID）</param>
        /// <param name="Key">批量键（列名）</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        long HashDelete(string hashId, string[] Key, string connectionName = null);
        /// <summary>
        /// 查询Hash中指定的key是否存在
        /// </summary>
        /// <param name="hashId">键Id(行ID）</param>
        /// <param name="Key">键（列名）</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        bool HashExists(string hashId, string Key, string connectionName = null);

    }
}
