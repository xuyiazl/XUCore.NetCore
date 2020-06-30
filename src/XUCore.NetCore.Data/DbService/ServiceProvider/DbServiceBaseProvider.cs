using XUCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace XUCore.NetCore.Data.DbService.ServiceProvider
{

    /// <summary>
    /// 数据库领域操作的基础对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbServiceBaseProvider<TEntity> : IDbServiceBase<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> dbRead { get; set; }

        public IBaseRepository<TEntity> dbWrite { get; set; }

        protected DbServiceBaseProvider(IBaseRepository<TEntity> readRepository, IBaseRepository<TEntity> writeRepository)
        {
            this.dbRead = readRepository;
            this.dbWrite = writeRepository;
        }

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作

        //同步操作

        /// <summary>
        /// 同步提交
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return dbWrite.SaveChanges();
        }
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Insert(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Insert(entity, isSaveChange);
            return -1;
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Insert(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Insert(entities, isSaveChange);
            return -1;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Update(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Update(entity, isSaveChange);
            return -1;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Update(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Update(entities, isSaveChange);
            return -1;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Delete(entity, isSaveChange);
            return -1;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <returns></returns>
        public virtual int Delete(TEntity[] entities, bool isSaveChange = true)
        {
            if (dbWrite != null)
                return dbWrite.Delete(entities, isSaveChange);
            return -1;
        }

        //异步操作

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await dbWrite.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// 异步插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.InsertAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.InsertAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.UpdateAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.UpdateAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TEntity entity, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.DeleteAsync(entity, isSaveChange, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSaveChange">是否提交</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TEntity[] entities, bool isSaveChange = true, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.DeleteAsync(entities, isSaveChange, cancellationToken);
            return -1;
        }

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            if (dbRead != null)
                return dbRead.GetById(id);
            return default;
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
        {
            if (dbRead != null)
                return dbRead.GetSingle(selector, orderby);
            return default;
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
        {
            if (dbRead != null)
                return dbRead.GetList(selector, orderby, skip, limit);
            return default;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="pageNumber">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public virtual PagedModel<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int pageNumber = 1, int pageSize = 10)
        {
            if (dbRead != null)
                return dbRead.GetPagedList(selector, orderby, pageNumber, pageSize);
            return default;
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> selector = null)
        {
            if (dbRead != null)
                return dbRead.Any(selector);
            return default;
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual long GetCount(Expression<Func<TEntity, bool>> selector = null)
        {
            if (dbRead != null)
                return dbRead.GetCount(selector);
            return default;
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
            if (dbRead != null)
                return await dbRead.GetByIdAsync(id, cancellationToken);
            return default;
        }
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetSingleAsync(selector, orderby, cancellationToken);
            return default;
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="pageNumber">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<PagedModel<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetPagedListAsync(selector, orderby, pageNumber, pageSize, cancellationToken);
            return default;
        }
        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.AnyAsync(selector, cancellationToken);
            return default;
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            if (dbRead != null)
                return await dbRead.GetCountAsync(selector, cancellationToken);
            return default;
        }

        #endregion

        #region 增加bulkextensions拓展

        //同步操作

        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="updateValues">更新的新数据数据</param>
        /// <param name="updateColumns">指定字段，如果需要更新为默认数据，那么需要指定字段，因为在内部实现会排除掉没有赋值的默认字段数据</param>
        /// <returns></returns>
        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null)
        {
            if (dbWrite != null)
                return dbWrite.BatchUpdate(selector, updateValues, updateColumns);
            return -1;
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        public virtual int BatchUpdate(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            if (dbWrite != null)
                return dbWrite.BatchUpdate(selector, Update);
            return -1;
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual int BatchDelete(Expression<Func<TEntity, bool>> selector)
        {
            if (dbWrite != null)
                return dbWrite.BatchDelete(selector);
            return -1;
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
        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchUpdateAsync(selector, updateValues, updateColumns, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchUpdateAsync(selector, Update, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> BatchDeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (dbWrite != null)
                return await dbWrite.BatchDeleteAsync(selector, cancellationToken);
            return -1;
        }

        #endregion


        #region [ AdoNet ]
        /// <summary>
        /// 通过EF执行原生SQL 返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, params DbParameter[] parameters)
        {
            if (dbWrite != null)
                return dbWrite.ExecuteSql(sql, parameters);
            return -1;
        }
        /// <summary>
        /// 通过ADO.NET通过EF执行原生SQL 返回影响行数 返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T Select<T>(string sql, CommandType type, params DbParameter[] parameters) where T : class, new()
        {
            if (dbRead != null)
                return dbRead.Select<T>(sql, type, parameters);
            return default;
        }
        /// <summary>
        /// 通过ADO.NET通过EF执行原生SQL 返回影响行数 返回查询结果集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<T> SelectList<T>(string sql, CommandType type, params DbParameter[] parameters) where T : class, new()
        {
            if (dbRead != null)
                return dbRead.SelectList<T>(sql, type, parameters);
            return default;
        }
        /// <summary>
        /// 通过ADO.NET通过EF执行原生SQL 返回影响行数 返回查询结果集合(DataTable)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable SelectList(string sql, CommandType type, params DbParameter[] parameters)
        {
            if (dbRead != null)
                return dbRead.SelectList(sql, type, parameters);
            return null;
        }
        /// <summary>
        /// 通过ADO.NET通过EF执行原生SQL 返回影响行数返回数据集(DataSet);
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataSet SelectDataSet(string sql, CommandType type, params DbParameter[] parameters)
        {
            if (dbRead != null)
                return dbRead.SelectDataSet(sql, type, parameters);
            return null;
        }
        /// <summary>
        /// 通过原生执行ADONET查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        public virtual int ExecuteAdoNet(string sql, CommandType type, params DbParameter[] parameters)
        {
            if (dbWrite != null)
                return dbWrite.ExecuteAdoNet(sql, type, parameters);
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
