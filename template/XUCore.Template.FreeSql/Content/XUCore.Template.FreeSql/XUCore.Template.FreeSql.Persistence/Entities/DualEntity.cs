using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence.Entities
{
    /// <summary>
    /// 系统表，用于查询系统函数
    /// </summary>
	[Table(Name = "sys_dual")]
    public class DualEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(Position = 1, IsPrimary = true, IsNullable = false)]
        public Guid Id { get; set; }
    }
}
