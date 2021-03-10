using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Paging
{
    /// <summary>
    /// 分页工具（用于数据同步等操作）
    /// </summary>
    public static class PagedTools
    {

        /// <summary>
        /// 根据页码获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public static long Page<T>(int size, Func<long> total, Func<int, int, IList<T>> data, Action<IList<T>> process)
        {
            var count = 0;
            var totalCount = total.Invoke();
            var pageCount = (int)Math.Ceiling(totalCount / (double)size);

            for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            {
                var page = data.Invoke(size, currentPage);

                if (page != null) count += page.Count;

                process.Invoke(page);
            }

            return count;
        }
        /// <summary>
        /// 根据页码获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<long> PageAsync<T>(int size,
            Func<CancellationToken, Task<long>> total,
            Func<int, int, CancellationToken, Task<IList<T>>> data,
            Func<IList<T>, CancellationToken, Task> process,
            CancellationToken cancellationToken = default)
        {
            var count = 0;
            var totalCount = await total.Invoke(cancellationToken);
            var pageCount = (int)Math.Ceiling(totalCount / (double)size);

            for (var currentPage = 1; currentPage <= pageCount; currentPage++)
            {
                var page = await data.Invoke(size, currentPage, cancellationToken);

                if (page != null) count += page.Count;

                await process.Invoke(page, cancellationToken);
            }

            return count;
        }
        /// <summary>
        /// 偏移获取数据分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="limit"></param>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public static long Offset<T>(int limit, Func<int, int, IList<T>> data, Action<IList<T>> process)
        {
            var total = 0;
            var skip = 0;

            while (true)
            {
                var _data = data.Invoke(skip, limit);

                if (_data == null || _data.Count == 0) return total;

                total += _data.Count;

                process.Invoke(_data);

                skip += limit;
            }
        }
        /// <summary>
        /// 偏移获取数据分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="limit"></param>
        /// <param name="data"></param>
        /// <param name="process"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<long> OffsetAsync<T>(int limit,
            Func<int, int, CancellationToken, Task<IList<T>>> data,
            Func<IList<T>, CancellationToken, Task> process,
            CancellationToken cancellationToken = default)
        {
            var total = 0;
            var skip = 0;

            while (true)
            {
                var _data = await data.Invoke(skip, limit, cancellationToken);

                if (_data == null || _data.Count == 0) return total;

                total += _data.Count;

                await process.Invoke(_data, cancellationToken);

                skip += limit;
            }
        }
    }


    /*
     
            PagedTools.Page<AdminUsersEntity>(
                size: 10,
                total: () => nigelDb.GetCount(),
                data: (pageSize, currentPage) => nigelDb.GetList(orderby: "Id asc", skip: pageSize * (currentPage - 1), limit: pageSize),
                process: (items) =>
                {
                    //do coding
                }
            );

            await PagedTools.PageAsync<AdminUsersEntity>(
                size: 10,
                total: async (cancel) => await nigelDb.GetCountAsync(cancellationToken: cancel),
                data: async (pageSize, currentPage, cancel) => await nigelDb.GetListAsync(orderby: "Id asc", skip: pageSize * (currentPage - 1), limit: pageSize, cancellationToken: cancel),
                process: async (items, cancel) =>
                {
                    //do coding

                    await Task.CompletedTask;
                },
                cancellationToken: cancellationToken
            );

            PagedTools.Offset<AdminUsersEntity>(
                limit: 10,
                data: (skip, limit) => nigelDb.GetList(orderby: "Id asc", skip: skip, limit: limit),
                process: (items) =>
                {
                    //do coding
                }
            );

            await PagedTools.OffsetAsync<AdminUsersEntity>(
                limit: 10,
                data: async (skip, limit, cancel) => await nigelDb.GetListAsync(orderby: "Id asc", skip: skip, limit: limit, cancellationToken: cancel),
                process: async (items, cancel) =>
                {
                    //do coding

                    await Task.CompletedTask;
                }
            );
     
     
     */
}
