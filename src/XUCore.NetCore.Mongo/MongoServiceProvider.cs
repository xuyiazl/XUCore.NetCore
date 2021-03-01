using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Configs;

namespace XUCore.NetCore.Mongo
{
    public class MongoServiceProvider<TMongoModel> : AbstractMongoBaseRepository<TMongoModel>, IMongoServiceProvider<TMongoModel> where TMongoModel : MongoBaseModel
    {
        public MongoServiceProvider(IConfiguration configuration) : base()
        {
            if (Connection == null || !Connection.IsValueCreated)
            {
                var ss = configuration.GetSection<List<MongoConnection>>("MongoConnectionStrings");
                Connection = new Lazy<List<MongoConnection>>(() =>
                {
                    return configuration.GetSection<List<MongoConnection>>("MongoConnectionStrings");
                });

            }
        }
    }
}
