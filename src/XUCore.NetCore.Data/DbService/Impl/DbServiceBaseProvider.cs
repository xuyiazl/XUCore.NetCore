using XUCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// 数据库领域操作的基础对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbServiceBaseProvider<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> dbRead { get; set; }

        public IBaseRepository<TEntity> dbWrite { get; set; }

        protected DbServiceBaseProvider(IBaseRepository<TEntity> readRepository, IBaseRepository<TEntity> writeRepository)
        {
            this.dbRead = readRepository;
            this.dbWrite = writeRepository;
        }

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作

        public int SaveChanges()
        {
            return dbWrite.SaveChanges();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await dbWrite.SaveChangesAsync(cancellationToken);
        }
        public virtual int Insert(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Insert(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.InsertAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchInsert(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.BatchInsert(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchInsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchInsertAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int Update(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Update(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.UpdateAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchUpdate(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.BatchUpdate(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchUpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchUpdateAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int Delete(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Delete(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.DeleteAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchDelete(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.BatchDelete(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchDeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchDeleteAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }



        public TEntity GetById(object id)
        {
            if (dbRead != null)
                return dbRead.GetById(id);
            return default;
        }
        public async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetByIdAsync(id, cancellationToken);
            return default;
        }
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression, string orderby)
        {
            if (dbRead != null)
                return dbRead.GetSingle(expression, orderby);
            return default;
        }
        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, string orderby, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetSingleAsync(expression, orderby);
            return default;
        }
        public virtual List<TEntity> GetList()
        {
            if (dbRead != null)
                return dbRead.GetList();
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(string orderby)
        {
            if (dbRead != null)
                return dbRead.GetList(orderby);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(string orderby, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(orderby, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector)
        {
            if (dbRead != null)
                return dbRead.GetList(selector);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(selector, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby)
        {
            if (dbRead != null)
                return dbRead.GetList(selector, orderby);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(selector, orderby, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20)
        {
            if (dbRead != null)
                return dbRead.GetList(selector, skip, limit);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(selector, skip, limit, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (dbRead != null)
                return dbRead.GetList(selector, orderby, skip, limit);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
        }
        public virtual PagedSkipModel<TEntity> GetPagedSkipList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (dbRead != null)
                return dbRead.GetPagedSkipList(selector, orderby, skip, limit);
            return default;
        }
        public virtual async Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetPagedSkipListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
        }
        public virtual PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            if (dbRead != null)
                return dbRead.GetPagedList(selector, orderby, pageNumber, pageSize);
            return default;
        }
        public virtual async Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetPagedListAsync(selector, orderby, pageNumber, pageSize, cancellationToken);
            return default;
        }
        public virtual bool Any(Expression<Func<TEntity, bool>> selector)
        {
            if (dbRead != null)
                return dbRead.Any(selector);
            return default;
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.AnyAsync(selector, cancellationToken);
            return default;
        }
        public virtual int GetCount(Expression<Func<TEntity, bool>> selector)
        {
            if (dbRead != null)
                return dbRead.GetCount(selector);
            return default;
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetCountAsync(selector, cancellationToken);
            return default;
        }

        #endregion

        #region 增加bulkextensions拓展

        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null)
        {
            if (dbWrite != null)
                return dbWrite.BatchUpdate(selector, updateValues, updateColumns);
            return -1;
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchUpdateAsync(selector, updateValues, updateColumns, cancellationToken);
            return -1;
        }

        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            if (dbWrite != null)
                return dbWrite.BatchUpdate(selector, Update);
            return -1;
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchUpdateAsync(selector, Update, cancellationToken);
            return -1;
        }

        public virtual int BatchDelete(Expression<Func<TEntity, bool>> selector)
        {
            if (dbWrite != null)
                return dbWrite.BatchDelete(selector);
            return -1;
        }

        public virtual async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchDeleteAsync(selector, cancellationToken);
            return -1;
        }

        #endregion

        #region 实现Dispose的方法

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ServiceBaseProvider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
