﻿using MessagePack;
using System;
using System.Collections.Generic;

namespace XUCore.Paging
{
    [MessagePackObject]
    public class PagedModel<T>
    {
        /// <summary>
        /// 初始化空对象
        /// </summary>
        public readonly static PagedModel<T> Empty = new PagedModel<T>(default(List<T>), 0, 1, 1);

        /// <summary>
        /// 页码
        /// </summary>
        [Key(0)]
        public int PageNumber { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [Key(1)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [Key(2)]
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [Key(3)]
        public long TotalRecords { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        [Key(4)]
        public IList<T> Items { get; set; }

        /// <summary>
        /// 附加信息（避免要返回多个模型数据的时候额外再项目中定义型模型）
        /// </summary>
        [Key(5)]
        public object ExtraInfo { get; set; }

        /// <summary>
        /// 是否能上一页
        /// </summary>
        [Key(6)]
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
        [Key(7)]
        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

        public PagedModel()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据</param>
        /// <param name="totalRecords">总记录数</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">分页大小</param>
        public PagedModel(IList<T> items, long totalRecords, int pageNumber, int pageSize)
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
        /// 创建一个空模型
        /// </summary>
        /// <param name="totalRecords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedModel<T> EmptyModel(long totalRecords, int pageIndex, int pageSize)
        {
            return new PagedModel<T>(default(List<T>), totalRecords, pageIndex, pageSize);
        }

    }
}