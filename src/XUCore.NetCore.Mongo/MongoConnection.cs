using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// mongo的连接配置
    /// </summary>
    public class MongoConnection
    {
        /// <summary>
        /// Mongo的连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 连接字符串别名
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// 获得当前的连接属性
        /// </summary>
        public Lazy<MongoClient> Connection { get; set; }
        /// <summary>
        /// 获得当前的数据库
        /// </summary>
        public IMongoDatabase Client
        {
            get
            {
                var builder = new MongoUrlBuilder(ConnectionString);
                return Connection.Value.GetDatabase(builder.DatabaseName);
            }
        }
        public MongoConnection()
        {
            try
            {
                this.Connection = new Lazy<MongoClient>(() =>
                {
                    var mongoclient = new MongoClient(ConnectionString);
                    return mongoclient;
                });
            }
            catch (Exception)
            {

            }
        }
    }
}
