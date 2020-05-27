using System;
using System.Collections.Generic;

namespace XUCore.Paging
{
    [Serializable]
    public class PagedModel<T>
    {
        /// <summary>
        /// 初始化空对象
        /// </summary>
        public readonly static PagedModel<T> Empty = new PagedModel<T>(default(List<T>), 0, 1, 1);

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public IList<T> Items { get; set; }

        public PagedModel()
        {
        }

        public PagedModel(IList<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (items != null && items.Count > 0)
                Items = items;
            else
                Items = new List<T>();
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