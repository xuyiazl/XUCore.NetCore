using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Extensions.Datas;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.Paging;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// 数据库的基础仓储库
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbRepository<TEntity> : SqlRepository, IDbRepository<TEntity> where TEntity : class, new()
    {
        protected string _connectionString { get; set; } = "";
        protected readonly IDbContext _context;
        protected readonly IUnitOfWork unitOfWork;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public DbRepository(IDbContext context) : base(context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
            unitOfWork = new UnitOfWorkService(context);
        }
        /// <summary>
        /// 当前上下文
        /// </summary>
        public IDbContext Context => _context;
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork => unitOfWork;
        /// <summary>
        /// 当前DbSet对象
        /// </summary>
        public DbSet<TEntity> Table => _context.Set<TEntity>();

        /// <summary>
        /// 转换上下文
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        public TDbContext As<TDbContext>() where TDbContext : IDbContext => _context.As<TDbContext>();

        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Add(TEntity entity, bool commit = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.Add(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<TEntity> entities, bool commit = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.AddRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity, bool commit = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.Update(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Update(IEnumerable<TEntity> entities, bool commit = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.UpdateRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity, bool commit = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.Remove(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool commit = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Table.RemoveRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(TEntity entity, bool commit = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await Table.AddAsync(entity, cancellationToken);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await Table.AddRangeAsync(entities, cancellationToken);

            if (commit) return unitOfWork.Commit();

            return 0;
        }

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return this.Table.Find(id);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
        {
            var query = Table.AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return query.AsNoTracking().FirstOrDefault();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
        {
            var query = Table.AsQueryable();

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
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public virtual PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10)
        {
            var totalCount = GetCount(selector);

            var list = GetList(selector, orderby, (currentPage - 1) * pageSize, pageSize);

            return new PagedList<TEntity>(list, totalCount, currentPage, pageSize);
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> selector = null)
        {
            if (selector == null)
                return Table.AsNoTracking().Any();

            return Table.AsNoTracking().Any(selector);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual long GetCount(Expression<Func<TEntity, bool>> selector = null)
        {
            if (selector == null)
                return Table.AsNoTracking().Count();

            return Table.AsNoTracking().Count(selector);
        }

        //异步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await this.Table.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
        {
            var query = Table.AsQueryable();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
        {
            var query = Table.AsQueryable();

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
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var totalCount = await GetCountAsync(selector, cancellationToken);

            var list = await GetListAsync(selector, orderby, (currentPage - 1) * pageSize, pageSize, cancellationToken);

            return new PagedList<TEntity>(list, totalCount, currentPage, pageSize);
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
                return await Table.AsNoTracking().AnyAsync(cancellationToken);

            return await Table.AnyAsync(selector, cancellationToken);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
                return await Table.AsNoTracking().CountAsync(cancellationToken);

            return await Table.AsNoTracking().CountAsync(selector, cancellationToken);
        }

        #region 增加bulkextensions拓展

        //同步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null)
        {
            return Table.Where(selector).BatchUpdate(updateValues, updateColumns);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            return Table.Where(selector).BatchUpdate(Update);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> selector)
        {
            return Table.Where(selector).BatchDelete();
        }

        //异步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            return await Table.Where(selector).BatchUpdateAsync(updateValues, updateColumns, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default)
        {
            return await Table.Where(selector).BatchUpdateAsync(Update, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            return await Table.Where(selector).BatchDeleteAsync(cancellationToken);
        }

        #endregion
    }
}
