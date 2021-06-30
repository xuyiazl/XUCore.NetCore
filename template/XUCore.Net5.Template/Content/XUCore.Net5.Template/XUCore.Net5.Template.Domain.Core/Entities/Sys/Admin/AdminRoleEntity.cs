
using System;
using System.Collections.Generic;

namespace XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin
{
    /// <summary>
    /// 角色二标
    /// </summary>
    public partial class AdminRoleEntity : BaseEntity
    {
        public AdminRoleEntity()
        {
            RoleMenus = new HashSet<AdminRoleMenuEntity>();
            UserRoles = new HashSet<AdminUserRoleEntity>();
        }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        public ICollection<AdminRoleMenuEntity> RoleMenus;
        /// <summary>
        /// 用户角色关联列表
        /// </summary>
        public ICollection<AdminUserRoleEntity> UserRoles;
    }
}
