﻿using XUCore.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using XUCore.Paging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// <see cref="IQueryable{T}"/> 扩展
    /// </summary>
    public static partial class QueryableExtensions
    {
        #region Take(Take扩展第三方条件)

        /// <summary>
        /// Take扩展，增加三方条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> Take<T>(this IQueryable<T> query, int count, bool condition)
        {
            if (!condition) return query;

            return query.Take(count);
        }

        #endregion

        #region WhereIf(是否执行指定条件的查询)

        /// <summary>
        /// 是否执行指定条件的查询，根据第三方条件是否为真来决定
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要查询的源</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="condition">第三方条件</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            bool condition)
        {
            source.CheckNotNull(nameof(source));
            predicate.CheckNotNull(nameof(predicate));

            return condition ? source.Where(predicate) : source;
        }

        #endregion

        #region PageBy(分页)

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="queryable">数据源</param>
        /// <param name="skipCount">跳过的行数</param>
        /// <param name="pageSize">每页记录数</param>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, int skipCount, int pageSize)
        {
            Check.NotNull(queryable, nameof(queryable));

            return queryable.Skip(skipCount).Take(pageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="TQueryable">查询源类型</typeparam>
        /// <param name="queryable">数据源</param>
        /// <param name="skipCount">跳过的行数</param>
        /// <param name="pageSize">每页记录数</param>
        public static TQueryable PageBy<T, TQueryable>(this IQueryable<T> queryable, int skipCount, int pageSize)
            where TQueryable : IQueryable
        {
            Check.NotNull(queryable, nameof(queryable));

            return (TQueryable)queryable.Skip(skipCount).Take(pageSize);
        }

        #endregion

        #region ToPagedList(分页)

        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="currentPage">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int currentPage, int pageSize)
        {
            var count = query.LongCount();

            var list = query.PageBy((currentPage - 1) * pageSize, pageSize).ToList();

            return new PagedList<T>(list, count, currentPage, pageSize);
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
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await query.LongCountAsync();

            var list = await query.PageBy((currentPage - 1) * pageSize, pageSize).ToListAsync(cancellationToken);

            return new PagedList<T>(list, count, currentPage, pageSize);
        }

        #endregion
    }
}