
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 角色表
    /// </summary>
	[Table(Name = "sys_role")]
    [Index("idx_{tablename}_01", nameof(Name), true)]
    public partial class RoleEntity : EntityFull
    {
        public RoleEntity()
        {
            RoleMenus = new List<RoleMenuEntity>();
            UserRoles = new List<UserRoleEntity>();
        }
        /// <summary>
        /// 角色名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        [Navigate(ManyToMany = typeof(RoleMenuEntity))]
        public ICollection<RoleMenuEntity> RoleMenus { get; set; }
        /// <summary>
        /// 用户角色关联列表
        /// </summary>

        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
