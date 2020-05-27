using System;
using System.Collections.Generic;

namespace XUCore.Paging
{
    [Serializable]
    public class PagedSkipModel<T>
    {
        public int Limit { get; set; }
        public int Skip { get; set; }
        public int TotalRecords { get; set; }

        public IList<T> Items { get; set; }

        public PagedSkipModel()
        {
        }

        public PagedSkipModel(IList<T> items, int totalRecords, int skip, int limit)
        {
            Limit = limit;
            Skip = skip;
            TotalRecords = totalRecords;

            if (items != null && items.Count > 0)
                Items = items;
            else
                Items = new List<T>();
        }
    }
}