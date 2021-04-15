using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.Mongo
{

    /// <summary>
    /// mongodb的基础仓库支持类
    /// </summary>
    public interface IMongoRepositoryBase
    {
        /// <summary>
        /// 获得数据Collection内容
        /// </summary>
        public IMongoCollection<TEntity> GetTable<TEntity>()
            where TEntity : MongoEntity;
        /// <summary>
        /// 获得具有bsonId的属性
        /// </summary>
        /// <returns></returns>
        public string GetBsonIds<TEntity>()
            where TEntity : MongoEntity;

        #region [新增]

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        TEntity Add<TEntity>(TEntity model, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync<TEntity>(TEntity model, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 同步批量添加
        /// </summary>
        /// <param name="models"></param>
        /// <param name="isOrdered">是否按顺序写入</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        void Add<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步批量添加
        /// </summary>
        /// <param name="models"></param>
        /// <param name="isOrdered">是否按顺序写入</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;

        #endregion

        #region [更新]
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where">条件</param>
        /// <param name="isUpsert">不存在时插入该文档</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        bool Update<TEntity>(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filter">条件</param>
        /// <param name="isUpsert">不存在时插入该文档</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        bool Update<TEntity>(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步更新
        /// </summary>
        Task<UpdateResult> UpdateAsync<TEntity>(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步更新
        /// </summary>
        Task<UpdateResult> UpdateAsync<TEntity>(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 批量更新
        /// </summary>
        bool Update<TEntity>(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步批量更新
        /// </summary>
        Task<BulkWriteResult<TEntity>> UpdateAsync<TEntity>(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 同步修改（部分字段）
        /// </summary>
        bool Update<TEntity>(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步修改（部分字段）
        /// </summary>
        Task<UpdateResult> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 批量修改（部分字段）
        /// </summary>
        bool Update<TEntity>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步批量修改（部分字段）
        /// </summary>
        Task<UpdateResult> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;

        #endregion

        #region 大批量操作

        /// <summary>
        /// 大批量写入
        /// </summary>
        BulkWriteResult<TEntity> BulkAdd<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步大批量写入
        /// </summary>
        Task<BulkWriteResult<TEntity>> BulkAddAsync<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 大批量操作
        /// </summary>
        BulkWriteResult<TEntity> BulkWrite<TEntity>(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步大批量操作
        /// </summary>
        Task<BulkWriteResult<TEntity>> BulkWriteAsync<TEntity>(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;

        #endregion

        #region [删除]

        /// <summary>
        /// 删除指定单一记录
        /// </summary>
        bool DeleteOne<TEntity>(TEntity t)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，删除
        /// </summary>
        TEntity FindOneAndDelete<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        Task<DeleteResult> DeleteOneAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        Task<DeleteResult> DeleteOneAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        bool Delete<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        Task<DeleteResult> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        Task<DeleteResult> DeleteAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;

        #endregion

        #region [查询]

        /// <summary>
        /// 获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        long GetCount<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        long GetCount<TEntity>(FilterDefinition<TEntity> filter)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获取指定主键id记录
        /// </summary>
        /// <returns></returns>
        TEntity GetById<TEntity>(object id)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获取指定主键id记录
        /// </summary>
        Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获得指定Linq条件内的单条数据
        /// </summary>
        TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获得指定Filter内的单条数据
        /// </summary>
        TEntity GetSingle<TEntity>(FilterDefinition<TEntity> filter)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获得指定Linq条件内的单条数据
        /// </summary>
        Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获得指定Filter内的单条数据
        /// </summary>
        Task<TEntity> GetSingleAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获取当前所有集合
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetList<TEntity>()
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获取当前所有集合
        /// </summary>
        /// <returns>返回异步结果</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获得指定Linq条件内的多条数据
        /// </summary>
        List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 获得指定Filter内的多条数据
        /// </summary>
        List<TEntity> GetList<TEntity>(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获得指定Linq条件内的多条数据
        /// </summary>
        Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步获得指定Filter内的多条数据
        /// </summary>
        Task<List<TEntity>> GetListAsync<TEntity>(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 分页获取数据
        /// </summary>
        PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize)
            where TEntity : MongoEntity;
        /// <summary>
        /// 分页获取数据
        /// </summary>
        PagedList<TEntity> GetPagedList<TEntity>(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;
        #endregion
    }
}
