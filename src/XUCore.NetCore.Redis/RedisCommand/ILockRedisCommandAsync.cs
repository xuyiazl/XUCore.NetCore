using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Redis.RedisCommand
{
    /// <summary>
    /// Lock 命令
    /// </summary>
    public interface ILockRedisCommandAsync
    {
        Task<bool> LockExtendAsync<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null);

        Task<T> LockQueryAsync<T>(string key, string connectionName = null, IRedisSerializer serializer = null);

        Task<bool> LockReleaseAsync<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null);

        Task<bool> LockTakeAsync<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null);

    }
}
