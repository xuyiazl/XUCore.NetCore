﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.Data
{

    /// <summary>
    /// 上下文仓储
    /// </summary>
    public interface IDbContextRepository<TDbContext> : ISqlRepository
        where TDbContext : IDbContext
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        TDbContext Context { get; }
        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        [Obsolete("命名已更改，请使用BuildFilter，将在下一版本删除")]
        Expression<Func<TEntity, bool>> AsQuery<TEntity>();
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> BuildFilter<TEntity>();
        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Add<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new();
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Add<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new();
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Update<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new();
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Update<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new();
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Delete<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new();
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        int Delete<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new();

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new();
        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById<TEntity>(object id) where TEntity : class, new();
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "") where TEntity : class, new();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0) where TEntity : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10) where TEntity : class, new();

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TDto GetById<TEntity, TDto>(object id)
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        TDto GetFirst<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        IList<TDto> GetList<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        PagedList<TDto> GetPagedList<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10)
            where TEntity : class, new()
            where TDto : class, new();

        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        bool Any<TEntity>(Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new();
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        long GetCount<TEntity>(Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new();

        //异步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default) where TEntity : class, new();

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDto> GetByIdAsync<TEntity, TDto>(object id, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDto> GetFirstAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TDto>> GetListAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedList<TDto>> GetPagedListAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new();

        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        #region 增加bulkextensions拓展

        //同步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <returns></returns>
        int Update<TEntity>(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null) where TEntity : class, new();
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        int Update<TEntity>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update) where TEntity : class, new();
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        int Delete<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, new();

        //异步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default) where TEntity : class, new();



        //void BulkAdd<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkAdd<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkAddOrUpdate<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkAddOrUpdate<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkAddOrUpdateOrDelete<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkAddOrUpdateOrDelete<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkUpdate<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkUpdate<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkDelete<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkDelete<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkRead<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new();

        //void BulkRead<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new();

        //void Truncate<TEntity>() where TEntity : class, new();

        //// Async methods

        //Task BulkAddAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkAddAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkAddOrUpdateAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkAddOrUpdateAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkAddOrUpdateOrDeleteAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkAddOrUpdateOrDeleteAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkReadAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task BulkReadAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new();

        //Task TruncateAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class, new();

        #endregion
    }
}
