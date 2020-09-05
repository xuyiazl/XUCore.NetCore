using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="pageSize"></param>
        /// <param name="totalFunc"></param>
        /// <param name="dataFunc"></param>
        /// <param name="dataAction"></param>
        /// <returns></returns>
        public static long Page<T>(int pageSize, Func<long> totalFunc, Func<int, int, IList<T>> dataFunc, Action<IList<T>> dataAction)
        {
            var total = 0;
            var totalCount = totalFunc.Invoke();
            var pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            for (var pageIndex = 1; pageIndex <= pageCount; pageIndex++)
            {
                var page = dataFunc.Invoke(pageSize, pageIndex);

                if (page != null) total += page.Count;

                dataAction.Invoke(page);
            }

            return total;
        }
        /// <summary>
        /// 偏移获取数据分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="limit"></param>
        /// <param name="dataFunc"></param>
        /// <param name="dataAction"></param>
        /// <returns></returns>
        public static long Offset<T>(int limit, Func<int, int, IList<T>> dataFunc, Action<IList<T>> dataAction)
        {
            var total = 0;
            var skip = 0;

            while (true)
            {
                var data = dataFunc.Invoke(skip, limit);

                if (data == null || data.Count == 0) return total;

                total += data.Count;

                dataAction.Invoke(data);
                skip += limit;
            }
        }
    }
}
