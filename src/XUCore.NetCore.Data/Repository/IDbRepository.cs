using XUCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using XUCore.NetCore.Data.BulkExtensions;

namespace XUCore.NetCore.Data
{

    /// <summary>
    /// 数据库仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDbRepository<TEntity> : ISqlRepository
        where TEntity : class, new()
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        IDbContext Context { get; }
        /// <summary>
        /// 当前DbSet对象
        /// </summary>
        DbSet<TEntity> Table { get; }

        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 转换上下文
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        TDbContext As<TDbContext>() where TDbContext : IDbContext;
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        Expression<Func<TEntity, bool>> AsQuery();

        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(TEntity entity, bool commit = true);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Add(IList<TEntity> entities, bool commit = true);
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity, bool commit = true);
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Update(IList<TEntity> entities, bool commit = true);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity, bool commit = true);
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Delete(IList<TEntity> entities, bool commit = true);

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddAsync(TEntity entity, bool commit = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddAsync(IList<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default);

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        TEntity GetFirst(Expression<Func<TEntity, bool>> selector = null, string orderby = "");
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10);
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> selector = null);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        long GetCount(Expression<Func<TEntity, bool>> selector = null);

        //异步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default);

        #region 增加bulkextensions拓展

        //同步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null);
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update);
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> selector);

        //异步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);



        //void BulkAdd(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkAdd(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void BulkAddOrUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkAddOrUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void BulkAddOrUpdateOrDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkAddOrUpdateOrDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void BulkUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void BulkDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void BulkRead(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null);

        //void BulkRead(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null);

        //void Truncate();

        //// Async methods

        //Task BulkAddAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkAddAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkAddOrUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkAddOrUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkAddOrUpdateOrDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkAddOrUpdateOrDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkReadAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task BulkReadAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default);

        //Task TruncateAsync(CancellationToken cancellationToken = default);

        #endregion

    }
}
