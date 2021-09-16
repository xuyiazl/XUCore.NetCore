using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XUCore.NetCore.FreeSql.Entity
{
    /// <summary>
    /// 实体修改
    /// </summary>
    public class EntityUpdate<TKey> : Entity<TKey>, IEntityUpdate<TKey>
    {
        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -3, CanInsert = false)]
        public long? ModifiedAtUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [Column(Position = -2, CanInsert = false), MaxLength(50)]
        public string ModifiedAtUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModifiedAt { get; set; }
    }

    public class EntityUpdate : EntityUpdate<long>
    {
    }
}