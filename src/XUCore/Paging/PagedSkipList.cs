
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Paging
{
    /// <summary>
    /// 分页列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedSkipList<T> : List<T>
    {
        /// <summary>
        /// 初始化空对象
        /// </summary>
        public readonly static PagedSkipList<T> Empty = new PagedSkipList<T>(default(List<T>), 0, 1, 1);

        /// <summary>
        /// 获取记录数
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedSkipList()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalRecords"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        public PagedSkipList(IList<T> items, int totalRecords, int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
            TotalRecords = totalRecords;
            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }
    }
}