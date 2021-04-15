using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Configs;

namespace XUCore.NetCore.Mongo
{
    public class MongoRepository<TEntity> : AbstractMongoRepository<TEntity>, IMongoRepository<TEntity> where TEntity : MongoEntity
    {
        public MongoRepository(IConfiguration configuration) : base()
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
