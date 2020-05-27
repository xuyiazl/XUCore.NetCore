using Microsoft.EntityFrameworkCore;
using XUCore.Extensions;
using XUCore.Paging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data
{
    public static class PagedExtensions
    {
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedModel<TResult> ToMap<T, TResult>(this PagedList<T> pagedList, Func<T, TResult> converter)
        {
            return new PagedModel<TResult>(pagedList.ForEach(converter), pagedList.TotalRecords, pagedList.PageNumber, pagedList.PageSize);
        }

        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedModel<TResult> ToMap<T, TResult>(this PagedList<T> pagedList, Func<T, int, TResult> converter)
        {
            return new PagedModel<TResult>(pagedList.ForEach(converter), pagedList.TotalRecords, pagedList.PageNumber, pagedList.PageSize);
        }

        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedSkipModel<TResult> ToMap<T, TResult>(this PagedSkipList<T> pagedList, Func<T, TResult> converter)
        {
            return new PagedSkipModel<TResult>(pagedList.ForEach(converter), pagedList.Limit, pagedList.Offset, pagedList.TotalRecords);
        }

        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pagedList"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static PagedSkipModel<TResult> ToMap<T, TResult>(this PagedSkipList<T> pagedList, Func<T, int, TResult> converter)
        {
            return new PagedSkipModel<TResult>(pagedList.ForEach(converter), pagedList.Limit, pagedList.Offset, pagedList.TotalRecords);
        }


        /// <summary>
        /// 创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> CreatePagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// 异步创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> CreatePagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize,
            CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }



        /// <summary>
        /// 创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static PagedSkipList<T> CreatePagedSkipList<T>(this IQueryable<T> source, int limit, int offset)
        {
            var count = source.Count();
            var items = source.Skip(offset).Take(limit).ToList();
            return new PagedSkipList<T>(items, limit, offset, count);
        }

        /// <summary>
        /// 异步创建分页对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedSkipList<T>> CreatePagedSkipListAsync<T>(this IQueryable<T> source, int limit, int offset,
            CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip(offset).Take(limit).ToListAsync(cancellationToken);
            return new PagedSkipList<T>(items, limit, offset, count);
        }
    }
}