namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// 定义Mongo的连接服务
    /// </summary>
    public interface IMongoRepository<TEntity> : IMongoRepositoryBase<TEntity> where TEntity : MongoEntity
    {

    }
}
