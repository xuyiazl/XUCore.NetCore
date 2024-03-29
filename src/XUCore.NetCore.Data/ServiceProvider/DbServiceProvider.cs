﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.Data
{

    /// <summary>
    /// 数据库领域操作的基础对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DbServiceProvider<TEntity> : IDbServiceProvider<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 只读对象
        /// </summary>
        public IDbRepository<TEntity> Read { get; }
        /// <summary>
        /// 只写对象
        /// </summary>
        public IDbRepository<TEntity> Write { get; }
        /// <summary>
        /// 当前DbSet对象
        /// </summary>
        public DbSet<TEntity> Table => Read.Table;

        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork => Write.UnitOfWork;

        protected DbServiceProvider(IDbRepository<TEntity> readRepository, IDbRepository<TEntity> writeRepository)
        {
            this.Read = readRepository;
            this.Write = writeRepository;
        }

        #region 抽象对象来实现IDbServiceBase中的方法，提供重写操作

        //同步操作

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Add(TEntity entity, bool commit = true)
        {
            return Write.Add(entity, commit);
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<TEntity> entities, bool commit = true)
        {
            return Write.Add(entities, commit);
        }
        /// <summary>
        /// 更新一条数据（全量更新）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity, bool commit = true)
        {
            return Write.Update(entity, commit);
        }
        /// <summary>
        /// 批量更新数据（全量更新）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Update(IEnumerable<TEntity> entities, bool commit = true)
        {
            return Write.Update(entities, commit);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity, bool commit = true)
        {
            return Write.Delete(entity, commit);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool commit = true)
        {
            return Write.Delete(entities, commit);
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
            return await Write.AddAsync(entity, commit, cancellationToken);
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
            return await Write.AddAsync(entities, commit, cancellationToken);
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
            return await Write.UpdateAsync(entity, commit);
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
            return await Write.UpdateAsync(entities, commit);
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
            return await Write.DeleteAsync(entity, commit);
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
            return await Write.DeleteAsync(entities, commit);
        }

        //同步查询

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            if (Read != null)
                return Read.GetById(id);
            return default;
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
        {
            if (Read != null)
                return Read.GetFirst(selector, orderby);
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
        public virtual IList<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
        {
            if (Read != null)
                return Read.GetList(selector, orderby, skip, limit);
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
        public virtual PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int pageNumber = 1, int pageSize = 10)
        {
            if (Read != null)
                return Read.GetPagedList(selector, orderby, pageNumber, pageSize);
            return default;
        }

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TDto GetById<TDto>(object id)
            where TDto : class, new()
        {
            if (Read != null)
                return Read.GetById<TDto>(id);
            return default;
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <returns></returns>
        public virtual TDto GetFirst<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "")
            where TDto : class, new()
        {
            if (Read != null)
                return Read.GetFirst<TDto>(selector);
            return default;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="skip">起始位置（默认为-1，不设置 一般从0开始）</param>
        /// <param name="limit">记录数（默认为0，不设置）</param>
        /// <returns></returns>
        public virtual IList<TDto> GetList<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0)
            where TDto : class, new()
        {
            if (Read != null)
                return Read.GetList<TDto>(selector, orderby, skip, limit);
            return default;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="currentPage">页码（最小为1）</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public virtual PagedList<TDto> GetPagedList<TDto>(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int currentPage = 1, int pageSize = 10)
            where TDto : class, new()
        {
            if (Read != null)
                return Read.GetPagedList<TDto>(selector, orderby, currentPage, pageSize);
            return default;
        }

        /// <summary>
        /// Any数据检测
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> selector = null)
        {
            if (Read != null)
                return Read.Any(selector);
            return default;
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual long GetCount(Expression<Func<TEntity, bool>> selector = null)
        {
            if (Read != null)
                return Read.GetCount(selector);
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
            if (Read != null)
                return await Read.GetByIdAsync(id, cancellationToken);
            return default;
        }
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="orderby">exp:“Id desc,CreateTime desc”</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", CancellationToken cancellationToken = default)
        {
            if (Read != null)
                return await Read.GetFirstAsync(selector, orderby, cancellationToken);
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
        public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> selector = null, string orderby = "", int skip = -1, int limit = 0, CancellationToken cancellationToken = default)
        {
            if (Read != null)
                return await Read.GetListAsync(selector, orderby, skip, limit, cancellationToken);
            return default;
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
            if (Read != null)
                return await Read.GetPagedListAsync(selector, orderby, currentPage, pageSize, cancellationToken);
            return default;
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
            if (Read != null)
                return await Read.GetByIdAsync<TDto>(id, cancellationToken);
            return default;
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
            if (Read != null)
                return await Read.GetFirstAsync<TDto>(selector, orderby, cancellationToken);
            return default;
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
            if (Read != null)
                return await Read.GetListAsync<TDto>(selector, orderby, skip, limit, cancellationToken);
            return default;
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
            if (Read != null)
                return await Read.GetPagedListAsync<TDto>(selector, orderby, currentPage, pageSize, cancellationToken);
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
            if (Read != null)
                return await Read.AnyAsync(selector, cancellationToken);
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
            if (Read != null)
                return await Read.GetCountAsync(selector, cancellationToken);
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
        public virtual int Update(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null)
        {
            if (Write != null)
                return Write.Update(selector, updateValues, updateColumns);
            return -1;
        }
        /// <summary>
        /// 根据条件批量更新（部分字段）
        /// </summary>
        /// <param name="selector">查询条件</param>
        /// <param name="Update">更新的新数据数据</param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TEntity, bool>> selector, Expression<Func<TEntity, TEntity>> Update)
        {
            if (Write != null)
                return Write.Update(selector, Update);
            return -1;
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> selector)
        {
            if (Write != null)
                return Write.Delete(selector);
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
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> selector, TEntity updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            if (Write != null)
                return await Write.UpdateAsync(selector, updateValues, updateColumns, cancellationToken);
            return -1;
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
            if (Write != null)
                return await Write.UpdateAsync(selector, Update, cancellationToken);
            return -1;
        }
        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        {
            if (Write != null)
                return await Write.DeleteAsync(selector, cancellationToken);
            return -1;
        }

        #endregion


        #region [ AdoNet ]


        public virtual T SqlFirst<T>(string sql, object model = null, CommandType type = CommandType.Text)
        {
            return Read.SqlFirst<T>(sql, model, type);
        }

        public virtual async Task<T> SqlFirstAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Read.SqlFirstAsync<T>(sql, model, type, cancellationToken);
        }

        public virtual IList<T> SqlQuery<T>(string sql, object model = null, CommandType type = CommandType.Text)
        {
            return Read.SqlQuery<T>(sql, model, type);
        }

        public virtual async Task<IList<T>> SqlQueryAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Read.SqlQueryAsync<T>(sql, model, type, cancellationToken);
        }

        public virtual DataTable SqlReader(string sql, object model = null, CommandType type = CommandType.Text)
            => Read.SqlReader(sql, model, type);

        public virtual async Task<DataTable> SqlReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await Read.SqlReaderAsync(sql, model, type, cancellationToken);

        public virtual DataSet SqlQueries(string sql, object model = null, CommandType type = CommandType.Text)
            => Read.SqlQueries(sql, model, type);

        public virtual async Task<DataSet> SqlQueriesAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await Read.SqlQueriesAsync(sql, model, type, cancellationToken);

        public virtual int SqlNonQuery(string sql, object model = null, CommandType type = CommandType.Text)
            => Write.SqlNonQuery(sql, model, type);

        public virtual async Task<int> SqlNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await Write.SqlNonQueryAsync(sql, model, type, cancellationToken);

        public virtual T SqlScalar<T>(string sql, object model = null, CommandType type = CommandType.Text)
            => Write.SqlScalar<T>(sql, model, type);

        public virtual async Task<T> SqlScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await Write.SqlScalarAsync<T>(sql, model, type, cancellationToken);


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
