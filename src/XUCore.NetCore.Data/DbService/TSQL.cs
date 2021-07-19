using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 常用T-SQL
    /// </summary>
    public static class TSQL
    {
        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="fields">需要查询的字段</param>
        /// <param name="tableName">表，可以是多表组合，视图</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="where">where条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public static (string PageSql, string CountSql) ToBetweenPageSql(string fields, string tableName, string where, string orderby, int pageNumber, int pageSize)
        {
            var pageSql = ToBetweenSql(fields, tableName, where, orderby, (pageNumber - 1) * pageSize + 1, pageNumber * pageSize);

            var countSql = ToCountSql(tableName, where);

            return (pageSql, countSql);
        }

        private static string ToBetweenSql(string fields, string tableName, string where, string orderby, int offset, int limit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1}) as _rid, {2} FROM {0} ", tableName, orderby, fields);

            if (!string.IsNullOrEmpty(where))
                strSql.AppendFormat("WHERE {0} ", where);

            strSql.AppendFormat(") as _t WHERE _t._rid BETWEEN {0} AND {1}", offset, offset + limit);

            return strSql.ToString();
        }

        private static string ToCountSql(string tableName, string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT COUNT(1) FROM {0} ", tableName);

            if (!string.IsNullOrEmpty(where))
                strSql.AppendFormat("WHERE {0}", where);

            return strSql.ToString();
        }
    }
}
