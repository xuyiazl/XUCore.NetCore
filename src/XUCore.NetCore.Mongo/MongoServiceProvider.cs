using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Configs;

namespace XUCore.NetCore.Mongo
{
    public class MongoServiceProvider<TMongoModel> : AbstractMongoBaseRepository<TMongoModel>, IMongoService<TMongoModel> where TMongoModel : MongoBaseModel
    {
        public MongoServiceProvider(
            IConfiguration configuration) : base()
        {
            #region 读取配置文件中的mongodb配置信息
            if (Connection == null || !Connection.IsValueCreated)
            {
                Connection = new Lazy<List<MongoConnection>>(() =>
                {
                    return configuration.GetSection<List<MongoConnection>>("MongoConnectionStrings");
                });

            }
            #endregion
        }
    }
}
