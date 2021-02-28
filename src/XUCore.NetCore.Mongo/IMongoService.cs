using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// 定义Mongo的连接服务
    /// </summary>
    public interface IMongoService<TMongoModel> : IMongoBaseRepository<TMongoModel> where TMongoModel : MongoBaseModel
    {
    }
}
