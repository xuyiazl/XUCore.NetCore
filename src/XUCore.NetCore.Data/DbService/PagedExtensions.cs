using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.NetCore.Data.DbService
{
    public static partial class PagedExtensions
    {
        public static PagedModel<T> CreatePagedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = query.LongCount();

            var list = query.PageBy((pageNumber - 1) * pageSize, pageSize).ToList();

            return new PagedModel<T>(list, count, pageNumber, pageSize);
        }

        public static async Task<PagedModel<T>> CreatePagedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.LongCountAsync();

            var list = await query.PageBy((pageNumber - 1) * pageSize, pageSize).ToListAsync();

            return new PagedModel<T>(list, count, pageNumber, pageSize);
        }
    }
}
