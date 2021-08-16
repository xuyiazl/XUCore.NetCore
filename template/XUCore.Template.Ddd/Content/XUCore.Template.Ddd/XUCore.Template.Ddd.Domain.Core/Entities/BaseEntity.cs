using System;
using XUCore.Ddd.Domain;

namespace XUCore.Template.Ddd.Domain.Core.Entities
{
    public class BaseEntity : BaseKeyEntity
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
        /// 创建人
        /// </summary>
        public string CreatedAtUserId { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public string UpdatedAtUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public string DeletedAtUserId { get; set; }
    }

    public class BaseKeyEntity : Entity<string>, IAggregateRoot
    {

    }
}
