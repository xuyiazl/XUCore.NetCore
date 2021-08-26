﻿using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Template.FreeSql.Persistence.Entities
{
    /// <summary>
    /// 实体完整类
    /// </summary>
    public class EntityFull<TKey> : Entity<TKey>, IEntityVersion, IEntitySoftDelete, IEntityAdd<TKey>, IEntityUpdate<TKey> where TKey : struct
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -9, IsVersion = true)]
        public long Version { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -8)]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        [Column(Position = -7, CanUpdate = false)]
        public long? CreatedAtUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [Column(Position = -6, CanUpdate = false), MaxLength(50)]
        public string CreatedAtUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -4, CanInsert = false)]
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

    /// <summary>
    /// 实体完整类
    /// </summary>
    public class EntityFull : EntityFull<long>
    {
    }
}