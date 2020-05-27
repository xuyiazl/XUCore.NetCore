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
        public readonly static PagedList<T> Empty = new PagedList<T>(default(List<T>), 0, 1, 1);

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; }

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
        public int TotalRecords { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedList()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据</param>
        /// <param name="totalRecords">总记录数</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        public PagedList(IList<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }

        /// <summary>
        /// 是否能上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        /// <summary>
        /// 是否能下一页
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

    }
}