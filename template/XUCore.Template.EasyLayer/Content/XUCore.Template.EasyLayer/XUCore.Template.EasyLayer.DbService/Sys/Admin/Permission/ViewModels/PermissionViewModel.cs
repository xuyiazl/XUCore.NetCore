using System.Collections.Generic;
using XUCore.Template.EasyLayer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
