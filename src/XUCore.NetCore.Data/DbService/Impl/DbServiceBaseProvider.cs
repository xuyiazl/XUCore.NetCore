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
        public IBaseRepository<TEntity> readRepository { get; set; }

        public IBaseRepository<TEntity> writeRepository { get; set; }

        protected DbServiceBaseProvider(IBaseRepository<TEntity> readRepository, IBaseRepository<TEntity> writeRepository)
        {
            this.readRepository = readRepository;
            this.writeRepository = writeRepository;
        }

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作

        public int SaveChanges()
        {
            return writeRepository.SaveChanges();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await writeRepository.SaveChangesAsync(cancellationToken);
        }
        public virtual int Insert(TEntity entity, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.Insert(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.InsertAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchInsert(TEntity[] entities, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.BatchInsert(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchInsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.BatchInsertAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int Update(TEntity entity, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.Update(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.UpdateAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchUpdate(TEntity[] entities, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.BatchUpdate(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchUpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.BatchUpdateAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int Delete(TEntity entity, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.Delete(entity, isSaveChange);
            return -1;
        }
        public virtual async Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.DeleteAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        public virtual int BatchDelete(TEntity[] entities, bool isSaveChange = true)
        {
            if (writeRepository != null)
                return writeRepository.BatchDelete(entities, isSaveChange);
            return -1;
        }
        public virtual async Task<int> BatchDeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.BatchDeleteAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }



        public TEntity GetById(object id)
        {
            if (readRepository != null)
                return readRepository.GetById(id);
            return default;
        }
        public async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetByIdAsync(id, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList()
        {
            if (readRepository != null)
                return readRepository.GetList();
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync( CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(string orderby)
        {
            if (readRepository != null)
                return readRepository.GetList(orderby);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(string orderby, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(orderby, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector)
        {
            if (readRepository != null)
                return readRepository.GetList(selector);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(selector, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby)
        {
            if (readRepository != null)
                return readRepository.GetList(selector, orderby);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(selector, orderby, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
                return readRepository.GetList(selector, skip, limit);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(selector, skip, limit, cancellationToken);
            return default;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
                return readRepository.GetList(selector, orderby, skip, limit);
            return default;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
        }
        public virtual PagedSkipModel<TEntity> GetPagedSkipList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            if (readRepository != null)
                return readRepository.GetPagedSkipList(selector, orderby, skip, limit);
            return default;
        }
        public virtual async Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetPagedSkipListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
        }
        public virtual PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20)
        {
            if (readRepository != null)
                return readRepository.GetPagedList(selector, orderby, pageNumber, pageSize);
            return default;
        }
        public virtual async Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetPagedListAsync(selector, orderby, pageNumber, pageSize, cancellationToken);
            return default;
        }
        public virtual bool Any(Expression<Func<TEntity, bool>> selector)
        {
            if (readRepository != null)
                return readRepository.Any(selector);
            return default;
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.AnyAsync(selector, cancellationToken);
            return default;
        }
        public virtual int GetCount(Expression<Func<TEntity, bool>> selector)
        {
            if (readRepository != null)
                return readRepository.GetCount(selector);
            return default;
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (readRepository != null)
                return await readRepository.GetCountAsync(selector, cancellationToken);
            return default;
        }

        #endregion

        #region 增加bulkextensions拓展

        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            if (writeRepository != null)
                return writeRepository.BatchUpdate(selector, Update);
            return -1;
        }

        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.BatchUpdateAsync(selector, Update, cancellationToken);
            return -1;
        }

        public virtual int BatchDelete(Expression<Func<TEntity, bool>> selector)
        {
            if (writeRepository != null)
                return writeRepository.BatchDelete(selector);
            return -1;
        }

        public virtual async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (writeRepository != null)
                return await writeRepository.BatchDeleteAsync(selector, cancellationToken);
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
