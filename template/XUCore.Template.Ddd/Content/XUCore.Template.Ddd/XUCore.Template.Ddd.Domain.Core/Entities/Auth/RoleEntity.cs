
using System;
using System.Collections.Generic;

namespace XUCore.Template.Ddd.Domain.Core.Entities.Auth
{
    /// <summary>
    /// 角色表
    /// </summary>
    public partial class RoleEntity : BaseEntity
    {
        public RoleEntity()
        {
            RoleMenus = new List<RoleMenuEntity>();
            UserRoles = new List<UserRoleEntity>();
        }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        public ICollection<RoleMenuEntity> RoleMenus;
        /// <summary>
        /// 用户角色关联列表
        /// </summary>
        public ICollection<UserRoleEntity> UserRoles;
    }
}
