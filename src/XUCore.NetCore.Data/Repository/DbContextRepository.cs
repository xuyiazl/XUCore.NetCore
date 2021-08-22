using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
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

namespace XUCore.NetCore.Data
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
        protected readonly IMapper _mapper;
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
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public DbContextRepository(TDbContext context, IMapper mapper) : base(context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
            _mapper = mapper;
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
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        public Expression<Func<TEntity, bool>> AsQuery<TEntity>() => c => true;
        /// <summary>
        /// 初始化查询表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> BuildFilter<TEntity>() => c => true;

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
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit">马上提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await Task.CompletedTask;

            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().Update(entity);

            if (commit)
                return unitOfWork.Commit();
            return 0;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await Task.CompletedTask;

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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync<TEntity>(TEntity entity, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await Task.CompletedTask;

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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities, bool commit = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            await Task.CompletedTask;

            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            _context.Set<TEntity>().RemoveRange(entities);

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
        public virtual TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "") where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10) where TEntity : class, new()
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
        public virtual TDto GetById<TEntity, TDto>(object id) 
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var res = this._context.Set<TEntity>().Find(id);

            return _mapper.Map<TEntity, TDto>(res);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TDto GetFirst<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual IList<TDto> GetList<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual PagedList<TDto> GetPagedList<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10)
            where TEntity : class, new()
            where TDto : class, new()
        {
            var totalCount = GetCount(selector);

            var list = GetList<TEntity, TDto>(selector, orderby, (currentPage - 1) * pageSize, pageSize);

            return new PagedList<TDto>(list, totalCount, currentPage, pageSize);
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
        public virtual async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default) where TEntity : class, new()
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
        public virtual async Task<TDto> GetByIdAsync<TEntity, TDto>(object id, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var res = await this._context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);

            return _mapper.Map<TEntity, TDto>(res);
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetFirstAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual async Task<IList<TDto>> GetListAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var query = _context.Set<TEntity>().AsNoTracking();

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
        public virtual async Task<PagedList<TDto>> GetPagedListAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            where TEntity : class, new()
            where TDto : class, new()
        {
            _mapper.CheckNull(nameof(IMapper));

            var totalCount = await GetCountAsync(selector, cancellationToken);

            var list = await GetListAsync<TEntity, TDto>(selector, orderby, (currentPage - 1) * pageSize, pageSize, cancellationToken);

            return new PagedList<TDto>(list, totalCount, currentPage, pageSize);
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

            return await _context.Set<TEntity>().AsNoTracking().AnyAsync(selector, cancellationToken);
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


        //public virtual void BulkAdd<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsert(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAdd<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsert<TEntity>(entities, bulkAction, progress);
        //}

        //public virtual void BulkAddOrUpdate<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdate(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAddOrUpdate<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdate(entities, bulkAction, progress);
        //}

        //public virtual void BulkAddOrUpdateOrDelete<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdateOrDelete(entities, bulkConfig, progress);
        //}

        //public virtual void BulkAddOrUpdateOrDelete<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkInsertOrUpdateOrDelete(entities, bulkAction, progress);
        //}

        //public virtual void BulkUpdate<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkUpdate(entities, bulkConfig, progress);
        //}

        //public virtual void BulkUpdate<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkUpdate(entities, bulkAction, progress);
        //}

        //public virtual void BulkDelete<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkDelete(entities, bulkConfig, progress);
        //}

        //public virtual void BulkDelete<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkDelete(entities, bulkAction, progress);
        //}

        //public virtual void BulkRead<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkRead(entities, bulkConfig, progress);
        //}

        //public virtual void BulkRead<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null) where TEntity : class, new()
        //{
        //    _context.As<DbContext>().BulkRead(entities, bulkAction, progress);
        //}

        //public virtual void Truncate<TEntity>() where TEntity : class, new()
        //{
        //    _context.As<DbContext>().Truncate<TEntity>();
        //}

        //// Async methods

        //public virtual Task BulkAddAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateOrDeleteAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateOrDeleteAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkAddOrUpdateOrDeleteAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkInsertOrUpdateOrDeleteAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkUpdateAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkUpdateAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkDeleteAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkDeleteAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task BulkReadAsync<TEntity>(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkReadAsync(entities, bulkConfig, progress, cancellationToken);
        //}

        //public virtual Task BulkReadAsync<TEntity>(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().BulkReadAsync(entities, bulkAction, progress, cancellationToken);
        //}

        //public virtual Task TruncateAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class, new()
        //{
        //    return _context.As<DbContext>().TruncateAsync<TEntity>(cancellationToken);
        //}


        #endregion

    }
}
