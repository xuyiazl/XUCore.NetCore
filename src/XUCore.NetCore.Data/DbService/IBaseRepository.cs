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
    /// 通用仓储库的方法定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        int Insert(T entity, bool isSaveChange = true);
        Task<int> InsertAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchInsert(T[] entities, bool isSaveChange = true);
        Task<int> BatchInsertAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Update(T entity, bool isSaveChange = true);
        Task<int> UpdateAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchUpdate(T[] entities, bool isSaveChange = true);
        Task<int> BatchUpdateAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Delete(T entity, bool isSaveChange = true);
        Task<int> DeleteAsync(T entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchDelete(T[] entities, bool isSaveChange = true);
        Task<int> BatchDeleteAsync(T[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        T GetById(object id);
        Task<T> GetByIdAsync(object id,CancellationToken cancellationToken = default);
        List<T> GetList();
        Task<List<T>> GetListAsync(CancellationToken cancellationToken = default);
        List<T> GetList(string orderby);
        Task<List<T>> GetListAsync(string orderby, CancellationToken cancellationToken = default);
        List<T> GetList(Expression<Func<T, bool>> selector);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken = default);
        List<T> GetList(Expression<Func<T, bool>> selector, string orderby);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, CancellationToken cancellationToken = default);
        List<T> GetList(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        List<T> GetList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedSkipModel<T> GetPagedSkipList(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedSkipModel<T>> GetPagedSkipListAsync(Expression<Func<T, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedModel<T> GetPagedList(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        Task<PagedModel<T>> GetPagedListAsync(Expression<Func<T, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        bool Any(Expression<Func<T, bool>> selector);
        Task<bool> AnyAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken = default);
        int GetCount(Expression<Func<T, bool>> selector);
        Task<int> GetCountAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken = default);



        #region 增加bulkextensions拓展

        int BatchUpdate(Expression<Func<T, bool>> selector, Expression<Func<T, T>> Update);

        Task<int> BatchUpdateAsync(Expression<Func<T, bool>> selector, Expression<Func<T, T>> Update, CancellationToken cancellationToken = default);

        int BatchDelete(Expression<Func<T, bool>> selector);

        Task<int> BatchDeleteAsync(Expression<Func<T, bool>> selector, CancellationToken cancellationToken = default);

        #endregion

    }
}
