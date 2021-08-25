using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.FreeSql
{
    /// <summary>
    /// ISelect 扩展
    /// </summary>
    public static class ISelectExtensions
    {
        #region ToPagedList(分页)

        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this ISelect<T> query, int currentPage, int pageSize)
        {
               var list = query.Count(out var count).Page(currentPage, pageSize).ToList<T>();

            return new PagedList<T>(list, count, currentPage, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PagedList<TDto> ToPagedList<T, TDto>(this ISelect<T> query, int currentPage, int pageSize)
        {
            var list = query.Count(out var count).Page(currentPage, pageSize).ToList<TDto>();

            return new PagedList<TDto>(list, count, currentPage, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="select"></param>
        /// <returns></returns>
        public static PagedList<TReturn> ToPagedList<T, TReturn>(this ISelect<T> query, int currentPage, int pageSize, Expression<Func<T, TReturn>> select)
        {
            var list = query.Count(out var count).Page(currentPage, pageSize).ToList(select);

            return new PagedList<TReturn>(list, count, currentPage, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this ISelect<T> query, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            var list = await query.Count(out var count).Page(currentPage, pageSize).ToListAsync<T>(cancellationToken);

            return new PagedList<T>(list, count, currentPage, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<TDto>> ToPagedListAsync<T, TDto>(this ISelect<T> query, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            var list = await query.Count(out var count).Page(currentPage, pageSize).ToListAsync<TDto>(cancellationToken);

            return new PagedList<TDto>(list, count, currentPage, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="select"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<TReturn>> ToPagedListAsync<T, TReturn>(this ISelect<T> query, int currentPage, int pageSize, Expression<Func<T, TReturn>> select, CancellationToken cancellationToken = default)
        {
            var list = await query.Count(out var count).Page(currentPage, pageSize).ToListAsync(select, cancellationToken);

            return new PagedList<TReturn>(list, count, currentPage, pageSize);
        }
        #endregion
    }
}
