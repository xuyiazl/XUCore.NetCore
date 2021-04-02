using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.Paging;

namespace XUCore.NetCore.Data.DbService
{
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void Add<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
                throw new ArgumentException($"{typeof(TEntity)} is Null");

            unitOfWork.Set<TEntity>().Add(entity);
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static void Add<TEntity>(this IUnitOfWork unitOfWork, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            unitOfWork.Set<TEntity>().AddRange(entities);
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void Update<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            unitOfWork.Set<TEntity>().Update(entity);
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static void Update<TEntity>(this IUnitOfWork unitOfWork, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            unitOfWork.Set<TEntity>().UpdateRange(entities);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void Delete<TEntity>(this IUnitOfWork unitOfWork, TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            unitOfWork.Set<TEntity>().Remove(entity);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static void Delete<TEntity>(this IUnitOfWork unitOfWork, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            unitOfWork.Set<TEntity>().RemoveRange(entities);
        }

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task AddAsync<TEntity>(this IUnitOfWork unitOfWork, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await unitOfWork.Set<TEntity>().AddAsync(entity);
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task AddAsync<TEntity>(this IUnitOfWork unitOfWork, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await unitOfWork.Set<TEntity>().AddRangeAsync(entities);
        }

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TEntity GetById<TEntity>(this IUnitOfWork unitOfWork, object id) where TEntity : class, new()
        {
            return unitOfWork.Set<TEntity>().Find(id);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public static TEntity GetSingle<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "") where TEntity : class, new()
        {
            var query = unitOfWork.Set<TEntity>().AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return query.AsNoTracking().FirstOrDefault();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public static List<TEntity> GetList<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0) where TEntity : class, new()
        {
            var query = unitOfWork.Set<TEntity>().AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return query.AsNoTracking().ToList();
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PagedList<TEntity> GetPagedLis<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10) where TEntity : class, new()
        {
            var totalCount = GetCount(unitOfWork, selector);

            var list = GetList(unitOfWork, selector, orderby, (currentPage - 1) * pageSize, pageSize);

            return new PagedList<TEntity>(list, totalCount, currentPage, pageSize);
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static bool Any<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new()
        {
            if (selector == null)
                return unitOfWork.Set<TEntity>().AsNoTracking().Any();

            return unitOfWork.Set<TEntity>().AsNoTracking().Any(selector);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static long GetCount<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new()
        {
            if (selector == null)
                return unitOfWork.Set<TEntity>().AsNoTracking().Count();

            return unitOfWork.Set<TEntity>().AsNoTracking().Count(selector);
        }

        //异步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TEntity> GetByIdAsync<TEntity>(this IUnitOfWork unitOfWork, object id, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await unitOfWork.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TEntity> GetSingleAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = unitOfWork.Set<TEntity>().AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetListAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = unitOfWork.Set<TEntity>().AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var totalCount = await GetCountAsync(unitOfWork, selector, cancellationToken);

            var list = await GetListAsync(unitOfWork, selector, orderby, (currentPage - 1) * pageSize, pageSize, cancellationToken);

            return new PagedList<TEntity>(list, totalCount, currentPage, pageSize);
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> AnyAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (selector == null)
                return await unitOfWork.Set<TEntity>().AsNoTracking().AnyAsync(cancellationToken);

            return await unitOfWork.Set<TEntity>().AnyAsync(selector, cancellationToken);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<long> GetCountAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (selector == null)
                return await unitOfWork.Set<TEntity>().AsNoTracking().CountAsync(cancellationToken);

            return await unitOfWork.Set<TEntity>().AsNoTracking().CountAsync(selector, cancellationToken);
        }

        #region 增加bulkextensions拓展

        //同步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <returns></returns>
        public static int Update<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null) where TEntity : class, new()
        {
            return unitOfWork.Set<TEntity>().Where(selector).BatchUpdate(updateValues, updateColumns);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        public static int Update<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update) where TEntity : class, new()
        {
            return unitOfWork.Set<TEntity>().Where(selector).BatchUpdate(Update);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static int Delete<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector) where TEntity : class, new()
        {
            return unitOfWork.Set<TEntity>().Where(selector).BatchDelete();
        }

        //异步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> UpdateAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await unitOfWork.Set<TEntity>().Where(selector).BatchUpdateAsync(updateValues, updateColumns, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> UpdateAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await unitOfWork.Set<TEntity>().Where(selector).BatchUpdateAsync(Update, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> DeleteAsync<TEntity>(this IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await unitOfWork.Set<TEntity>().Where(selector).BatchDeleteAsync(cancellationToken);
        }

        #endregion
    }
}
