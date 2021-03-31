namespace XUCore.NetCore.Data.DbService
{
    public interface IMySqlRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, new()
    {

    }
}
