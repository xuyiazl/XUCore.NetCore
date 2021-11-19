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
    public abstract partial class StackExchangeRedis : IListRedisCommand
    {
        public long ListLeftPushWhenExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListLeftPush(key, serializer.Serializer(value), When.Exists, CommandFlags.None);
                return db.ListLeftPush(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public long ListLeftPush<T>(string key, List<T> value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null || value.Count == 0) return 0;
                RedisValue[] values = new RedisValue[value.Count];

                for (int i = 0; i < value.Count; i++)
                    if (serializer != null)
                        values[i] = serializer.Serializer(value[i]);
                    else
                        values[i] = redisSerializer.Serializer(value[i]);

                return db.ListLeftPush(key, values, CommandFlags.None);
            });
        }

        public long ListLeftPushWhenNoExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListLeftPush(key, serializer.Serializer(value), When.Always, CommandFlags.None);
                return db.ListLeftPush(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
            });
        }

        public long ListRightPushWhenExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListRightPush(key, serializer.Serializer(value), When.Exists, CommandFlags.None);
                return db.ListRightPush(key, redisSerializer.Serializer(value), When.Exists, CommandFlags.None);
            });
        }

        public long ListRightPushWhenNoExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListRightPush(key, serializer.Serializer(value), When.Always, CommandFlags.None);
                return db.ListRightPush(key, redisSerializer.Serializer(value), When.Always, CommandFlags.None);
            });
        }

        public long ListRightPush<T>(string key, List<T> value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (value == null || value.Count == 0) return 0;
                RedisValue[] values = new RedisValue[value.Count];
                for (int i = 0; i < value.Count; i++)
                {
                    if (serializer != null)
                        values[i] = serializer.Serializer(value[i]);
                    else
                        values[i] = redisSerializer.Serializer(value[i]);
                }
                return db.ListRightPush(key, values, CommandFlags.None);
            });
        }

        public T ListLeftPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.ListLeftPop(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public T ListRightPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.ListRightPop(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public long ListLength(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.ListLength(key);
            });
        }

        public void ListSetByIndex<T>(string key, long index, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    db.ListSetByIndex(key, index, serializer.Serializer(value));
                else
                    db.ListSetByIndex(key, index, redisSerializer.Serializer(value));
            });
        }

        public void ListTrim(string key, long index, long end, string connectionName = null)
        {
            ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                db.ListTrim(key, index, end);
            });
        }

        public T ListGetByIndex<T>(string key, long index, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.ListGetByIndex(key, index);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public IList<T> ListRange<T>(string key, long start, long end, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var result = db.ListRange(key, start, end);
                if (serializer != null)
                    return serializer.Deserialize<T>(result);
                return redisSerializer.Deserialize<T>(result);
            });
        }

        public long ListInsertBefore<T>(string key, T value, string insertvalue, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListInsertBefore(key, serializer.Serializer(value), insertvalue);
                return db.ListInsertBefore(key, redisSerializer.Serializer(value), insertvalue);
            });
        }

        public long ListInsertAfter<T>(string key, T value, string insertvalue, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListInsertAfter(key, serializer.Serializer(value), insertvalue);
                return db.ListInsertAfter(key, redisSerializer.Serializer(value), insertvalue);
            });
        }

        public long ListRemove<T>(string key, T value, long removecount, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.ListRemove(key, serializer.Serializer(value), removecount);
                return db.ListRemove(key, redisSerializer.Serializer(value), removecount);
            });
        }
    }
}
