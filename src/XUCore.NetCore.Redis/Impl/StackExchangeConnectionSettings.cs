using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// Redis连接配置
    /// </summary>
    public class StackExchangeConnectionSettings
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// 连接类型
        /// </summary>
        public ConnectTypeEnum ConnectType { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        ///// <summary>
        ///// 默认数据库
        ///// </summary>
        public int DefaultDb { get; set; }

        /// <summary>
        /// 连接Redis的密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 缓存操作对象
        /// </summary>
        public Lazy<ConnectionMultiplexer> Connection
        {
            get; set;
        }

        public Lazy<ConnectionMultiplexer> Multiplexer
        {
            get
            {
                return Connection;
            }
        }
        public StackExchangeConnectionSettings()
        {
            try
            {
                //配置连接
                this.Connection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(
                        new ConfigurationOptions()
                        {
                            EndPoints = { { this.EndPoint, int.Parse(this.Port) } },
                            ClientName = this.ConnectionName,
                            DefaultDatabase = this.DefaultDb,
                            AbortOnConnectFail = false,
                            ConnectRetry = 3,
                            ConnectTimeout = 3000,
                            //Proxy=Proxy.Twemproxy,
                            SyncTimeout = 1200,
                            KeepAlive = 60,
                            //KeepAlive = 60,
                            //ConnectTimeout = 60,
                            Password = this.Password,
                            DefaultVersion = new Version(2, 8, 19)
                        });
                });


            }
            catch (RedisConnectionException ex)
            {
                throw new RedisException(this.EndPoint, this.Port, ex.Message, "缓存服务未开启导致的错误,请开启缓存服务后刷新");
            }
        }
    }
}
