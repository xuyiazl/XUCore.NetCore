using System;
using XUCore.Ddd.Domain;
using XUCore.WebApi2.Template.Core.Enums;

namespace XUCore.WebApi2.Template.Persistence.Entities
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
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }

}
