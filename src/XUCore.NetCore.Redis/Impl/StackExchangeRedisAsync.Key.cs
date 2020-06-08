using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using XUCore.Extensions;
using XUCore.Serializer;
using System.Threading.Tasks;
using XUCore.NetCore.Redis.RedisCommand;

namespace XUCore.NetCore.Redis
{
    public abstract partial class StackExchangeRedis : IKeyRedisCommandAsync
    {
        public async Task<bool> KeyExistsAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.KeyExistsAsync(key);
            });
        }

        public async Task<byte[]> KeyDumpAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.KeyDumpAsync(key);
            });
        }

        public async Task<bool> KeyExpireAsync(string key, int seconds, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.KeyExpireAsync(key, TimeSpan.FromSeconds(seconds));
            });
        }

        public async Task<bool> KeyDeleteAsync(string Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.KeyDeleteAsync(Key);
            });
        }

        public async Task<bool> KeyPersistAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.KeyPersistAsync(key);
            });
        }
    }
}
