using XUCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// 通用仓储库的方法定义
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        int Insert(TEntity entity, bool isSaveChange = true);
        Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchInsert(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchInsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Update(TEntity entity, bool isSaveChange = true);
        Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchUpdate(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchUpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int Delete(TEntity entity, bool isSaveChange = true);
        Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default);
        int BatchDelete(TEntity[] entities, bool isSaveChange = true);
        Task<int> BatchDeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default);
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        TEntity GetSingle(Expression<Func<TEntity, bool>> expression, string orderby);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> expression, string orderby, CancellationToken cancellationToken = default);
        List<TEntity> GetList();
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
        List<TEntity> GetList(string orderby);
        Task<List<TEntity>> GetListAsync(string orderby, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedSkipModel<TEntity> GetPagedSkipList(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedSkipModel<TEntity>> GetPagedSkipListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int skip = 0, int limit = 20, CancellationToken cancellationToken = default);
        PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20);
        Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        bool Any(Expression<Func<TEntity, bool>> selector);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);
        int GetCount(Expression<Func<TEntity, bool>> selector);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);



        #region 增加bulkextensions拓展

        int BatchUpdate(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null);

        Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default);

        int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update);

        Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default);

        int BatchDelete(Expression<Func<TEntity, bool>> selector);

        Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default);

        #endregion

        #region [ AdoNet ]
        /// <summary>
        /// 通过EF执行原生SQL 返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, params DbParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T Select<T>(string sql, CommandType type, params DbParameter[] parameters) where T : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> SelectList<T>(string sql, CommandType type, params DbParameter[] parameters) where T : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合(DataTable)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SelectList(string sql, CommandType type, params DbParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL返回数据集(DataSet);
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet SelectDataSet(string sql, CommandType type, params DbParameter[] parameters);
        /// <summary>
        /// 通过原生执行ADONET查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        int ExecuteAdoNet(string sql, CommandType type, params DbParameter[] parameters);
        #endregion
    }
}
