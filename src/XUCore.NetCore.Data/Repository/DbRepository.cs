using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace XUCore.NetCore.Data
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
        protected readonly IMapper _mapper;
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
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public DbRepository(IDbContext context, IMapper mapper) : base(context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
            _mapper = mapper;
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
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        public Expression<Func<TEntity, bool>> AsQuery() => c => true;

        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Add(TEntity entity, bool commit = true)
        {
            entity.CheckNull(nameof(TEntity));

            Table.Add(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<TEntity> entities, bool commit = true)
        {
            entities.CheckNull(nameof(IEnumerable<TEntity>));

            Table.AddRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity, bool commit = true)
        {
            entity.CheckNull(nameof(TEntity));

            Table.Update(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Update(IEnumerable<TEntity> entities, bool commit = true)
        {
            entities.CheckNull(nameof(IEnumerable<TEntity>));

            Table.UpdateRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity, bool commit = true)
        {
            entity.CheckNull(nameof(TEntity));

            Table.Remove(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool commit = true)
        {
            entities.CheckNull(nameof(IEnumerable<TEntity>));

            Table.RemoveRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }

        //异步操作

        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(TEntity entity, bool commit = true, CancellationToken cancellationToken = default)
        {
            entity.CheckNull(nameof(TEntity));

            await Table.AddAsync(entity, cancellationToken);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default)
        {
            entities.CheckNull(nameof(IEnumerable<TEntity>));

            await Table.AddRangeAsync(entities, cancellationToken);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity entity, bool commit = true, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            entity.CheckNull(nameof(TEntity));

            Table.Update(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            entities.CheckNull(nameof(IEnumerable<TEntity>));

            Table.UpdateRange(entities);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TEntity entity, bool commit = true, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            entity.CheckNull(nameof(TEntity));

            Table.Remove(entity);

            if (commit) return unitOfWork.Commit();

            return 0;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            entities.CheckNull(nameof(IEnumerable<TEntity>));

            Table.RemoveRange(entities);

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
            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return query.FirstOrDefault();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
        {
            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return query.ToList();
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
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TDto GetById<TDto>(object id) where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var res = this.Table.Find(id);

            return _mapper.Map<TEntity, TDto>(res);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TDto GetFirst<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "") where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return query.ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefault();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public virtual IList<TDto> GetList<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0) where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToList();
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public virtual PagedList<TDto> GetPagedList<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10) where TDto : class, new()
        {
            var totalCount = GetCount(selector);

            var list = GetList<TDto>(selector, orderby, (currentPage - 1) * pageSize, pageSize);

            return new PagedList<TDto>(list, totalCount, currentPage, pageSize);
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
            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return await query.FirstOrDefaultAsync(cancellationToken);
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
        public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
        {
            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return await query.ToListAsync(cancellationToken);
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
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetByIdAsync<TDto>(object id, CancellationToken cancellationToken = default)
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var res = await this.Table.FindAsync(new object[] { id }, cancellationToken: cancellationToken);

            return _mapper.Map<TEntity, TDto>(res);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetFirstAsync<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            return await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
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
        public virtual async Task<IList<TDto>> GetListAsync<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = Table.AsNoTracking();

            if (selector != null)
                query = query.Where(selector);

            if (!string.IsNullOrEmpty(orderby))
                query = query.OrderByBatch(orderby);

            if (skip > -1)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
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
        public virtual async Task<PagedList<TDto>> GetPagedListAsync<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var totalCount = await GetCountAsync(selector, cancellationToken);

            var list = await GetListAsync<TDto>(selector, orderby, (currentPage - 1) * pageSize, pageSize, cancellationToken);

            return new PagedList<TDto>(list, totalCount, currentPage, pageSize);
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

            return await Table.AsNoTracking().AnyAsync(selector, cancellationToken);
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


        //public virtual void BulkAdd(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsert(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAdd(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsert(entities, bulkAction, progress);
        //}

        //public virtual void BulkAddOrUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdate(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAddOrUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdate(entities, bulkAction, progress);
        //}

        //public virtual void BulkAddOrUpdateOrDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdateOrDelete(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAddOrUpdateOrDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdateOrDelete(entities, bulkAction, progress);
        //}

        //public virtual void BulkUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkUpdate(entities, bulkConfig, progress);
        //}

        //public virtual void BulkUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkUpdate(entities, bulkAction, progress);
        //}

        //public virtual void BulkDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkDelete(entities, bulkConfig, progress);
        //}

        //public virtual void BulkDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkDelete(entities, bulkAction, progress);
        //}

        //public virtual void BulkRead(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkRead(entities, bulkConfig, progress);
        //}

        //public virtual void BulkRead(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null)
        //{
        //    _context.As<DbContext>().BulkRead(entities, bulkAction, progress);
        //}

        //public virtual void Truncate()
        //{
        //    _context.As<DbContext>().Truncate<TEntity>();
        //}

        //// Async methods

        //public virtual Task BulkAddAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateOrDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateOrDeleteAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateOrDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateOrDeleteAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkUpdateAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkUpdateAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkDeleteAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkDeleteAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkReadAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkReadAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkReadAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().BulkReadAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task TruncateAsync(CancellationToken cancellationToken = default)
        //{
        //    return _context.As<DbContext>().TruncateAsync<TEntity>(cancellationToken);
        //}

        #endregion
    }
}
