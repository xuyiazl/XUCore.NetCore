﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Extensions.Datas;
using XUCore.Helpers;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.Paging;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// 数据库的基础仓储库
    /// </summary>
    public abstract class DbContextRepository<TDbContext> : SqlRepository, IDbContextRepository<TDbContext>
        where TDbContext : IDbContext
    {
        protected string _connectionString { get; set; } = "";
        protected readonly TDbContext _context;
        protected readonly IUnitOfWork unitOfWork;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public DbContextRepository(TDbContext context) : base(context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
            unitOfWork = new UnitOfWorkService(context);
        }
        /// <summary>
        /// 当前上下文
        /// </summary>
        public TDbContext Context => _context;

        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork => unitOfWork;

        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        public virtual int Add<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().Add(entity);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        public virtual int Add<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().AddRange(entities);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        public virtual int Update<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().Update(entity);

            if (commit)
                return unitOfWork.Commit();
            return 0;
            /// </summary>
            /// <param name="entities"></param>
            /// <param name="commit">马上提交</param>
            /// <returns></returns>
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        public virtual int Update<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().UpdateRange(entities);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        public virtual int Delete<TEntity>(TEntity entity, bool commit = true) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().Remove(entity);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <returns></returns>
        public virtual int Delete<TEntity>(IEnumerable<TEntity> entities, bool commit = true) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().RemoveRange(entities);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById<TEntity>(object id) where TEntity : class, new()
        {
            return this._context.Set<TEntity>().Find(id);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "") where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsQueryable();

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
        public virtual List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsQueryable();

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
        public virtual PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10) where TEntity : class, new()
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
        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new()
        {
            if (selector == null)
                return _context.Set<TEntity>().AsNoTracking().Any();

            return _context.Set<TEntity>().AsNoTracking().Any(selector);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual long GetCount<TEntity>(Expression<Func<TEntity, bool>> selector = null) where TEntity : class, new()
        {
            if (selector == null)
                return _context.Set<TEntity>().AsNoTracking().Count();

            return _context.Set<TEntity>().AsNoTracking().Count(selector);
        }

        //异步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await this._context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsQueryable();

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
        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsQueryable();

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
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default) where TEntity : class, new()
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
        public virtual async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (selector == null)
                return await _context.Set<TEntity>().AsNoTracking().AnyAsync(cancellationToken);

            return await _context.Set<TEntity>().AnyAsync(selector, cancellationToken);
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            if (selector == null)
                return await _context.Set<TEntity>().AsNoTracking().CountAsync(cancellationToken);

            return await _context.Set<TEntity>().AsNoTracking().CountAsync(selector, cancellationToken);
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
        public virtual int Update<TEntity>(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null) where TEntity : class, new()
        {
            return _context.Set<TEntity>().Where(selector).BatchUpdate(updateValues, updateColumns);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        public virtual int Update<TEntity>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update) where TEntity : class, new()
        {
            return _context.Set<TEntity>().Where(selector).BatchUpdate(Update);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual int Delete<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class, new()
        {
            return _context.Set<TEntity>().Where(selector).BatchDelete();
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
        public virtual async Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _context.Set<TEntity>().Where(selector).BatchUpdateAsync(updateValues, updateColumns, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _context.Set<TEntity>().Where(selector).BatchUpdateAsync(Update, cancellationToken);
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _context.Set<TEntity>().Where(selector).BatchDeleteAsync(cancellationToken);
        }

        #endregion


        public virtual TEntity SqlFirstOrDefault<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new()
        {
            var res = SqlQuery<TEntity>(sql, model, type);

            return res.Count > 0 ? res[0] : default;
        }

        public virtual async Task<TEntity> SqlFirstOrDefaultAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var res = await SqlQueryAsync<TEntity>(sql, model, type, cancellationToken);

            return res.Count > 0 ? res[0] : default;
        }

        public virtual IList<TEntity> SqlQuery<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new()
        {
            return ExecuteReader(sql, model, type).ToList<TEntity>();
        }

        public virtual async Task<IList<TEntity>> SqlQueryAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var res = await ExecuteReaderAsync(sql, model, type, cancellationToken);

            return res.ToList<TEntity>();
        }

        public virtual DataTable ExecuteReader(string sql, object model = null, CommandType type = CommandType.Text)
            => _context.Database.ExecuteReader(sql, model, type);

        public virtual async Task<DataTable> ExecuteReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await _context.Database.ExecuteReaderAsync(sql, model, type, cancellationToken);

        public virtual DataSet DataAdapterFill(string sql, object model = null, CommandType type = CommandType.Text)
            => _context.Database.DataAdapterFill(sql, model, type);

        public virtual async Task<DataSet> DataAdapterFillAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await _context.Database.DataAdapterFillAsync(sql, model, type, cancellationToken);

        public virtual int ExecuteNonQuery(string sql, object model = null, CommandType type = CommandType.Text)
            => _context.Database.ExecuteNonQuery(sql, model, type);

        public virtual async Task<int> ExecuteNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await _context.Database.ExecuteNonQueryAsync(sql, model, type, cancellationToken);

        public virtual T ExecuteScalar<T>(string sql, object model = null, CommandType type = CommandType.Text)
            => _context.Database.ExecuteScalar<T>(sql, model, type);

        public virtual async Task<T> ExecuteScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await _context.Database.ExecuteScalarAsync<T>(sql, model, type, cancellationToken);
    }
}