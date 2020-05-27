using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Redis Key 命令
    /// </summary>
    public interface IKeyRedisCommand
    {
        /// <summary>
        /// 判断key是否存在，某些操作需要创建新的对象而不是修改老对象的情况下面请先使用该方法判断key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        bool KeyExists(string key, string connectionName = null);
        /// <summary>
        /// 序列化给定 key ，并返回被序列化的值，使用 RESTORE 命令可以将这个值反序列化为 Redis 键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        byte[] KeyDump(string key, string connectionName = null);
        /// <summary>
        /// 设置键失效时间，当时间一到自动移除key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds">失效时间为秒（s）</param>
        /// <param name="connectionName">连接名称</param>
        bool KeyExpire(string key, int seconds, string connectionName = null);
        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="Key">key名称</param>
        /// <param name="connectionName">连接名称</param>
        /// <returns></returns>
        bool KeyDelete(string Key, string connectionName = null);
        /// <summary>
        /// 移除Key设定的生命周期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        bool KeyPersist(string key, string connectionName = null);
    }
}
