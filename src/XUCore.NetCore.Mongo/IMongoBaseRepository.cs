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
    /// mongodb的基础仓库
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IMongoBaseRepository<TModel> where TModel : MongoBaseModel
    {
        /// <summary>
        /// 获得具有bsonId的属性
        /// </summary>
        /// <returns></returns>
        string GetBsonIds();
        /// <summary>
        /// 获得数据Collection内容
        /// </summary>
        IMongoCollection<TModel> Table { get;}
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        TModel Add(TModel model, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TModel> AddAsync(TModel model, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 同步批量添加
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        void AddMany(IEnumerable<TModel> models, bool isOrdered = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步批量添加
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isOrdered">获取或设置一个值，该值指示请求是否按顺序添加。</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        Task<int> AddManyAsync(IEnumerable<TModel> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        bool Update(TModel model, Expression<Func<TModel, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <returns></returns>
        bool Update(TModel model, FilterDefinition<TModel> filter, bool isUpsert = true, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateAsync(TModel model, Expression<Func<TModel, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model">对象信息</param>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateAsync(TModel model, FilterDefinition<TModel> filter, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        bool UpdateMany(IEnumerable<TModel> models, Expression<Func<TModel, bool>> where = null, bool isUpsert = true);
        /// <summary>
        /// 异步批量更新
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="where">设置一个更新条件，默认：匹配BsonId；</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="cancellationToken"></param>
        Task<BulkWriteResult<TModel>> UpdateManyAsync(IEnumerable<TModel> models, Expression<Func<TModel, bool>> where = null, bool isUpsert = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// 同步修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="field">指定字段</param>
        /// <param name="value">更新值</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        bool UpdateFiled(Expression<Func<TModel, bool>> where, string field, string value, bool? bypassDocumentValidation = null);
        /// <summary>
        /// 异步修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="field">指定字段</param>
        /// <param name="value">更新值</param>
        /// <param name="bypassDocumentValidation">获取或设置一个值，该值指示是否绕过文档验证。</param>
        /// <param name="cancellationToken"></param>
        Task<UpdateResult> UpdateFiledAsync(Expression<Func<TModel, bool>> where, string field, string value, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量修改（部分字段）
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新字段</param>
        /// <returns></returns>
        bool Update(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TModel>> update);
        /// <summary>
        /// 异步批量修改（部分字段）
        /// <param name="where">更新条件</param>
        /// <param name="update">更新字段</param>
        /// <param name="cancellationToken"></param>
        /// </summary>
        Task<UpdateResult> UpdateAsync(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TModel>> update, CancellationToken cancellationToken = default);
        /// <summary>
        /// 大批量写入
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        bool BulkWrite(IEnumerable<WriteModel<TModel>> models, bool isUpsert = true);
        /// <summary>
        /// 异步大批量写入
        /// </summary>
        /// <param name="models">对象信息</param>
        /// <param name="isUpsert">获取或设置一个值，该值指示如果不存在，是否插入</param>
        /// <param name="cancellationToken"></param>
        Task<BulkWriteResult<TModel>> BulkWriteAsync(IEnumerable<WriteModel<TModel>> models, bool isUpsert = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除指定单一记录
        /// </summary>
        /// <remarks>默认按当前对象主键ID删除</remarks>
        /// <param name="t">删除对象</param>
        bool Delete(TModel t);
        /// <summary>
        /// 按条件，删除
        /// </summary>
        /// <param name="where">条件</param>
        TModel Delete(Expression<Func<TModel, bool>> where);
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteAsync(Expression<Func<TModel, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        bool DeleteMany(Expression<Func<TModel, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        bool DeleteMany(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteManyAsync(Expression<Func<TModel, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        Task<DeleteResult> DeleteManyAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定Linq条件总记录数
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <returns></returns>
        long GetCount(Expression<Func<TModel, bool>> where);
        /// <summary>
        /// 获取指定Filter条件总记录数
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// </summary>
        /// <returns></returns>
        long GetCount(FilterDefinition<TModel> filter);
        /// <summary>
        /// 异步获取指定Linq条件总记录数
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(Expression<Func<TModel, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获取指定Filter条件总记录数
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        /// </summary>
        /// <returns></returns>
        Task<long> GetCountAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取当前所有集合
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IList<TModel> GetList(CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获取当前所有集合
        /// </summary>
        /// <returns>返回异步结果</returns>
        /// <param name="cancellationToken"></param>
        Task<List<TModel>> GetListAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定主键id记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        TModel GetById(object id);
        /// <summary>
        /// 异步获取指定主键id记录
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TModel> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="selector">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <returns></returns>
        PagedModel<TModel> GetPagedList(Expression<Func<TModel, bool>> selector, string orderby, int pageIndex, int pageSize);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <returns></returns>
        PagedModel<TModel> GetPagedList(FilterDefinition<TModel> filter, string orderby, int pageIndex, int pageSize);
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        /// <param name="selector">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<TModel>> GetPagedListAsync(Expression<Func<TModel, bool>> selector, string orderby, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页尺码</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<TModel>> GetPagedListAsync(FilterDefinition<TModel> filter, string orderby, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获得指定Linq条件内的多条数据
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <returns></returns>
        List<TModel> GetList(Expression<Func<TModel, bool>> where, string orderby = "", int? limit = null);
        /// <summary>
        /// 获得指定Filter内的多条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <returns></returns>
        List<TModel> GetList(FilterDefinition<TModel> filter, string orderby = "", int? limit = null);
        /// <summary>
        /// 异步获得指定Linq条件内的多条数据
        /// </summary>
        /// <param name="where">筛选条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> where, string orderby = "", int? limit = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获得指定Filter内的多条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="orderby">多个OrderBy用逗号隔开，exp:"name asc,createtime desc"</param>
        /// <param name="limit">指定条数</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<List<TModel>> GetListAsync(FilterDefinition<TModel> filter, string orderby = "", int? limit = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获得指定Linq条件内的单条数据
        /// </summary>
        /// <param name="where">linq表达式</param>
        /// <returns></returns>
        TModel GetSingle(Expression<Func<TModel, bool>> where);
        /// <summary>
        /// 获得指定Filter内的单条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <returns></returns>
        TModel GetSingle(FilterDefinition<TModel> filter);
        /// <summary>
        /// 异步获得指定Linq条件内的单条数据
        /// </summary>
        /// <param name="where">筛选条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TModel> GetSingleAsync(Expression<Func<TModel, bool>> where, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步获得指定Filter内的单条数据
        /// </summary>
        /// <param name="filter">基础过滤器，请使用Builders构建条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns>返回异步结果</returns>
        Task<TModel> GetSingleAsync(FilterDefinition<TModel> filter, CancellationToken cancellationToken = default);
    }
}
