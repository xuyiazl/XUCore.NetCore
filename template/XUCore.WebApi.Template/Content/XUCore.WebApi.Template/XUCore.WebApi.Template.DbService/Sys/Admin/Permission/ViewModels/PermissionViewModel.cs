﻿using System.Collections.Generic;
using XUCore.WebApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi.Template.DbService.Sys.Admin.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}