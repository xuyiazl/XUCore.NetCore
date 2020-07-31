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
    public abstract partial class StackExchangeRedis : ISortSetRedisCommand
    {
        public bool SortedAdd<T>(string key, T value, double score, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SortedSetAdd(key, serializer.Serializer(value), score);
                return db.SortedSetAdd(key, redisSerializer.Serializer(value), score);
            });
        }

        public long SortedAdd<T>(string key, Dictionary<T, double> values, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                List<SortedSetEntry> sortedEntry = new List<SortedSetEntry>();
                foreach (var keyvalue in values)
                {
                    if (serializer != null)
                    {
                        var entry = new SortedSetEntry(serializer.Serializer(keyvalue.Key), keyvalue.Value);
                        sortedEntry.Add(entry);
                    }
                    else
                    {
                        var entry = new SortedSetEntry(redisSerializer.Serializer(keyvalue.Key), keyvalue.Value);
                        sortedEntry.Add(entry);
                    }
                }
                return db.SortedSetAdd(key, sortedEntry.ToArray());
            });
        }

        public long SortedCount(string key, double start, double end, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.SortedSetLength(key, start, end);
            });
        }

        public long SortedRemove<T>(string key, IList<T> values, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                List<RedisValue> listValues = new List<RedisValue>();
                foreach (var val in values)
                {
                    if (serializer != null)
                        listValues.Add(serializer.Serializer(val));
                    listValues.Add(redisSerializer.Serializer(val));
                }

                return db.SortedSetRemove(key, listValues.ToArray());
            });
        }

        public bool SortedRemove<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SortedSetRemove(key, serializer.Serializer(value));
                return db.SortedSetRemove(key, redisSerializer.Serializer(value));
            });
        }

        public long SortedRemove(string key, long start, long stop, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.SortedSetRemoveRangeByRank(key, start, stop);
            });
        }

        public IList<T> SortedRangeByScore<T>(string key, double start, double stop, Exclude exclude = Exclude.None, Order orderby = Order.Ascending, int skip = 0, int take = -1, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var resultEntry = db.SortedSetRangeByScore(key, start, stop, exclude: exclude, order: orderby, skip: skip, take: take);

                if (serializer != null)
                    return serializer.Deserialize<T>(resultEntry);
                return redisSerializer.Deserialize<T>(resultEntry);
            });
        }

        public Dictionary<T, double> SortedRange<T>(string key, long start, long stop, Order orderby = Order.Ascending, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var resultEntry = db.SortedSetRangeByRankWithScores(key, start, stop, order: orderby);

                if (serializer != null)
                    return resultEntry.ToDictionary(t => serializer.Deserialize<T>(t.Element), t => t.Score);
                return resultEntry.ToDictionary(t => redisSerializer.Deserialize<T>(t.Element), t => t.Score);
            });
        }

        public long? SortedZrank<T>(string key, T value, Order orderby = Order.Ascending, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SortedSetRank(key, serializer.Serializer(value), orderby);
                return db.SortedSetRank(key, redisSerializer.Serializer(value), orderby);
            });
        }
    }
}
