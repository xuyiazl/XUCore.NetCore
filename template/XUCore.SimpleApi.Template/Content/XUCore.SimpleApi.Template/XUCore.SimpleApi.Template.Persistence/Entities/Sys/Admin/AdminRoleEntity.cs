﻿
using System;
using System.Collections.Generic;

namespace XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin
{
    /// <summary>
    /// 角色表
    /// </summary>
    public partial class AdminRoleEntity : BaseEntity
    {
        public AdminRoleEntity()
        {
            RoleMenus = new List<AdminRoleMenuEntity>();
            UserRoles = new List<AdminUserRoleEntity>();
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
