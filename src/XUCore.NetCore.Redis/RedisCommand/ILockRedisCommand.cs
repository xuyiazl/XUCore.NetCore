using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Lock 命令
    /// </summary>
    public interface ILockRedisCommand
    {
        bool LockExtend<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null);

        T LockQuery<T>(string key, string connectionName = null, IRedisSerializer serializer = null);

        bool LockRelease<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);

        bool LockTake<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null);
    }
}
