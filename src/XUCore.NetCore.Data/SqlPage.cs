using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.NetCore.Data
{
    /// <summary>
    /// sql分页
    /// </summary>
    public class SqlPage
    {
        private readonly ISqlRepository sqlRepository;
        /// <summary>
        /// sql分页
        /// </summary>
        /// <param name="sqlRepository"></param>
        public SqlPage(ISqlRepository sqlRepository)
        {
            this.sqlRepository = sqlRepository;
        }
        /// <summary>
        /// 需要查询的字段
        /// </summary>
        public string Fields { get; set; } = "*";
        /// <summary>
        /// 表，可以是多表组合，视图
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// where条件
        /// </summary>
        public string Where { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Orderby { get; set; }
        /// <summary>
        /// 分页SQL
        /// </summary>
        public virtual string PageSql(int begin, int end)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1}) as __rid__, {2} FROM {0} ", Table, Orderby, Fields);

            if (!string.IsNullOrEmpty(Where))
                strSql.AppendFormat("WHERE {0} ", Where);

            strSql.AppendFormat(") as _t WHERE _t.__rid__ BETWEEN {0} AND {1}", begin, end);

            return strSql.ToString();
        }
        /// <summary>
        /// count SQL
        /// </summary>
        public virtual string CountSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT COUNT(1) FROM {0} ", Table);

            if (!string.IsNullOrEmpty(Where))
                strSql.AppendFormat("WHERE {0}", Where);

            return strSql.ToString();
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public PagedList<TEntity> GetPage<TEntity>(int currentPage, int pageSize, object model = null) where TEntity : class, new()
        {
            var offset = (currentPage - 1) * pageSize + 1;
            var limit = currentPage * pageSize;

            var items = sqlRepository.SqlQuery<TEntity>(PageSql(offset, offset + limit), model);

            var count = sqlRepository.SqlScalar<long>(CountSql(), model);

            return new PagedList<TEntity>(items, count, currentPage, pageSize);
        }
    }
}
