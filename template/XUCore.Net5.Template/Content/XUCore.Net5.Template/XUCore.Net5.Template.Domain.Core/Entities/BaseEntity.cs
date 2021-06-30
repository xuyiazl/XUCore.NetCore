using System;
using XUCore.Ddd.Domain;

namespace XUCore.Net5.Template.Domain.Core.Entities
{
    public class BaseEntity : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created_At { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? Updated_At { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? Deleted_At { get; set; }
    }

}
