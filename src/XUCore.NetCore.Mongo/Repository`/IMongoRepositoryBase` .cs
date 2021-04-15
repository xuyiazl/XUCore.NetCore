using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// 定义Mongo的连接服务
    /// </summary>
    public interface IMongoRepositoryBase<TEntity> where TEntity : MongoEntity
    {
        /// <summary>
        /// 获得具有bsonId的属性
        /// </summary>
        /// <returns></returns>
        string GetBsonIds();
        /// <summary>
        /// 获得数据Collection内容
        /// </summary>
        IMongoCollection<TEntity> Table { get; }

        #region [ 新增 ]

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        TEntity Add(TEntity model, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity model, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 同步批量添加
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        void Add(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步批量添加
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        Task AddAsync(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);

        #endregion

        #region [ 更新 ]

        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        bool Update(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        bool Update(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateAsync(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateAsync(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        bool Update(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true);
        /// <summary>
        /// 异步批量更新
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="cancellationToken"></param>
        Task<BulkWriteResult<TEntity>> UpdateAsync(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// 同步修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="field">指定字段</param>
        /// <param name="value">更新值</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        bool Update(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="field">指定字段</param>
        /// <param name="value">更新值</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        Task<UpdateResult> UpdateAsync(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新字段</param>
        /// <returns></returns>
        bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update);
        /// <summary>
        /// 异步批量修改（部分字段）
        /// <param name="where">更新条件</param>
        /// <param name="update">更新字段</param>
        /// <param name="cancellationToken"></param>
        /// </summary>
        Task<UpdateResult> UpdateAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update, CancellationToken cancellationToken = default);

        #endregion

        #region [ 大批量操作 ]

        /// <summary>
        /// 大批量写入
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        BulkWriteResult<TEntity> BulkAdd(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步大批量写入
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        Task<BulkWriteResult<TEntity>> BulkAddAsync(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 大批量操作
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        BulkWriteResult<TEntity> BulkWrite(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步大批量操作
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<BulkWriteResult<TEntity>> BulkWriteAsync(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        #endregion

        #region [ 删除 ]

        /// <summary>
        /// 删除指定单一记录
        /// </summary>
        /// <remarks>默认按当前对象主键ID删除</remarks>
        /// <param name="t">删除对象</param>
        bool DeleteOne(TEntity t);
        /// <summary>
        /// 按条件，删除
        /// </summary>
        /// <param name="where">条件</param>
        TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteOneAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteOneAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        bool Delete(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        bool Delete(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);

        #endregion

        #region [ 查询 ]

        /// <summary>
        /// 获取指定Linq条件总记录数
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <returns></returns>
        long GetCount(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 获取指定Filter条件总记录数
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// </summary>
        /// <returns></returns>
        long GetCount(FilterDefinition<TEntity> filter);
        /// <summary>
        /// 异步获取指定Linq条件总记录数
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获取指定Filter条件总记录数
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        /// </summary>
        /// <returns></returns>
        Task<long> GetCountAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定主键id记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        TEntity GetById(object id);
        /// <summary>
        /// 异步获取指定主键id记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获得指定Linq条件内的单条数据
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <returns></returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 获得指定Filter内的单条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <returns></returns>
        TEntity GetSingle(FilterDefinition<TEntity> filter);
        /// <summary>
        /// 异步获得指定Linq条件内的单条数据
        /// </summary>
        /// <param name="where">筛选条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获得指定Filter内的单条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TEntity> GetSingleAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取当前所有集合
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetList();
        /// <summary>
        /// 异步获取当前所有集合
        /// </summary>
        /// <returns>返回异步结果</returns>
        /// <param name="cancellationToken"></param>
        Task<IList<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获得指定Linq条件内的多条数据
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <returns></returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null);
        /// <summary>
        /// 获得指定Filter内的多条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <returns></returns>
        List<TEntity> GetList(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null);
        /// <summary>
        /// 异步获得指定Linq条件内的多条数据
        /// </summary>
        /// <param name="where">筛选条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获得指定Filter内的多条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<List<TEntity>> GetListAsync(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="selector">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <returns></returns>
        PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <returns></returns>
        PagedList<TEntity> GetPagedList(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize);
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        /// <param name="selector">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> GetPagedListAsync(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default);

        #endregion
    }
}
