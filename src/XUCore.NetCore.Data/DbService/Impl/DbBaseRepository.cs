using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.Extensions;
using XUCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// 数据库的基础仓储库
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbBaseRepository<TEntity> where TEntity : class, new()
    {
        protected string _connectionString { get; set; } = "";
        protected readonly IBaseContext _context;
        protected DbSet<TEntity> _entities { get; set; }
        public DbBaseRepository(IBaseContext context)
        {
            _connectionString = context.ConnectionStrings;
            _context = context;
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public virtual int Insert(TEntity entity, bool isSaveChange = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.Add(entity);

            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            await Entities.AddAsync(entity);

            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int BatchInsert(TEntity[] entities, bool isSaveChange = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            //自增ID操作会出现问题，暂时无法解决自增操作的方式，只能使用笨办法，通过多次连接数据库的方式执行
            //var changeRecord = 0;
            //foreach (var item in entities)
            //{
            //    var entry = Entities.Add(item);
            //    entry.State = EntityState.Added;
            //    changeRecord += _context.SaveChanges();
            //}

            Entities.AddRange(entities);

            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> BatchInsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            //自增ID操作会出现问题，暂时无法解决自增操作的方式，只能使用笨办法，通过多次连接数据库的方式执行
            //var changeRecord = 0;
            //foreach (var item in entities)
            //{
            //    var entry = Entities.Add(item);
            //    entry.State = EntityState.Added;
            //    changeRecord += _context.SaveChanges();
            //}

            await Entities.AddRangeAsync(entities);

            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int Update(TEntity entity, bool isSaveChange = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.Update(entity);

            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.Update(entity);

            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int BatchUpdate(TEntity[] entities, bool isSaveChange = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.UpdateRange(entities);

            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> BatchUpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.UpdateRange(entities);

            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int Delete(TEntity entity, bool isSaveChange = true)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.Remove(entity);

            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.Remove(entity);

            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }
        public virtual int BatchDelete(TEntity[] entities, bool isSaveChange = true)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.RemoveRange(entities);
            if (isSaveChange)
                return SaveChanges();
            return 0;
        }
        public virtual async Task<int> BatchDeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentException($"{typeof(TEntity)} is Null");
            }

            Entities.RemoveRange(entities);
            if (isSaveChange)
                return await SaveChangesAsync(cancellationToken);
            return 0;
        }

        public TEntity GetById(object id)
        {
            return this.Entities.Find(id);
        }
        public async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await this.Entities.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression, string orderby)
        {
            return Entities.AsNoTracking().Where(expression).OrderByBatch(orderby).FirstOrDefault();
        }
        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, string orderby, CancellationToken cancellationToken = default)
        {
            return await Entities.AsNoTracking().Where(expression).OrderByBatch(orderby).FirstOrDefaultAsync();
        }
        public virtual List<TEntity> GetList()
        {
            return Entities.AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await Entities.AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual List<TEntity> GetList(string orderby)
        {
            return Entities.OrderByBatch(orderby).AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(string orderby, CancellationToken cancellationToken = default)
        {
            return await Entities.OrderByBatch(orderby).AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector)
        {
            return Entities.Where(selector).AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby)
        {
            return Entities.Where(selector).OrderByBatch(orderby).AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).OrderByBatch(orderby).AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).Skip(skip).Take(limit).AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).Skip(skip).Take(limit).AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return Entities.Where(selector).OrderByBatch(orderby).Skip(skip).Take(limit).AsNoTracking().ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).OrderByBatch(orderby).Skip(skip).Take(limit).AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual PagedSkipModel<TEntity> GetPagedSkipList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            var totalRecords = GetCount(selector);

            var list = GetList(selector, orderby, skip, limit);

            return new PagedSkipModel<TEntity>(list, totalRecords, skip, limit);
        }
        public virtual async Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            var totalRecords = await GetCountAsync(selector, cancellationToken);

            var list = await GetListAsync(selector, orderby, skip, limit, cancellationToken);

            return new PagedSkipModel<TEntity>(list, totalRecords, skip, limit);
        }
        public virtual PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            var totalRecords = GetCount(selector);

            var list = GetList(selector, orderby, (pageNumber - 1) * pageSize, pageSize);

            return new PagedModel<TEntity>(list, totalRecords, pageNumber, pageSize);
        }
        public virtual async Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var totalRecords = await GetCountAsync(selector, cancellationToken);

            var list = await GetListAsync(selector, orderby, (pageNumber - 1) * pageSize, pageSize, cancellationToken);

            return new PagedModel<TEntity>(list, totalRecords, pageNumber, pageSize);
        }
        public virtual bool Any(Expression<Func<TEntity, bool>> selector)
        {
            return Entities.Where(selector).Any();
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).AnyAsync(cancellationToken);
        }
        public virtual int GetCount(Expression<Func<TEntity, bool>> selector)
        {
            return Entities.AsNoTracking().Count(selector);
        }
        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            return Entities.AsNoTracking().CountAsync(selector, cancellationToken);
        }
        private DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<TEntity>();
                }

                return _entities;
            }
        }


        #region 增加bulkextensions拓展

        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null)
        {
            return Entities.Where(selector).BatchUpdate(updateValues, updateColumns);
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).BatchUpdateAsync(updateValues, updateColumns, cancellationToken);
        }


        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            return Entities.Where(selector).BatchUpdate(Update);
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).BatchUpdateAsync(Update, cancellationToken);
        }

        public virtual int BatchDelete(Expression<Func<TEntity, bool>> selector)
        {
            return Entities.Where(selector).BatchDelete();
        }

        public virtual async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(selector).BatchDeleteAsync(cancellationToken);
        }

        #endregion
    }
}
