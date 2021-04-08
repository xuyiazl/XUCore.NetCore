using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static partial class PagedExtensions
    {
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PagedList<T> CreatePagedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = query.LongCount();

            var list = query.PageBy((pageNumber - 1) * pageSize, pageSize).ToList();

            return new PagedList<T>(list, count, pageNumber, pageSize);
        }
        /// <summary>
        /// 创建分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> CreatePagedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await query.LongCountAsync();

            var list = await query.PageBy((pageNumber - 1) * pageSize, pageSize).ToListAsync(cancellationToken);

            return new PagedList<T>(list, count, pageNumber, pageSize);
        }
    }
}
