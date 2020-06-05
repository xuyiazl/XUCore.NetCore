using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Configs;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// Redis的组件服务提供
    /// </summary>
    public class StackExchangeRedisProvider : StackExchangeRedis, IStackExchangeRedis
    {
        public StackExchangeRedisProvider(IConfiguration configuration, IRedisSerializer redisSerializer) : base(configuration, redisSerializer)
        {
            if (connMultiplexer == null || !connMultiplexer.IsValueCreated)
            {
                connMultiplexer = new Lazy<List<StackExchangeConnectionSettings>>(() =>
                {
                    var config = configuration.GetSection<List<StackExchangeConnectionSettings>>("StackExchangeConnectionSettings");

                    return config;
                });
            }
        }
    }
}
