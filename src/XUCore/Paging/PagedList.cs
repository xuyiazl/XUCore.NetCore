namespace XUCore.Paging
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:35:40
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// 初始化空对象
        /// </summary>
        public readonly static PagedList<T> Empty = new PagedList<T>(new List<T>(), 0, 1, 1);

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedList() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        public PagedList(IList<T> items, long totalCount, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }

        /// <summary>
        /// 是否能上一页
        /// </summary>
        public bool HasPreviousPage => (CurrentPage > 1);

        /// <summary>
        /// 是否能下一页
        /// </summary>
        public bool HasNextPage => (CurrentPage < TotalPages);

        /// <summary>
        /// PagedModel模型转换（方便对外输出分页模型）
        /// </summary>
        public PagedModel<T> Model => new PagedModel<T>(this, TotalCount, CurrentPage, PageSize);
    }
}