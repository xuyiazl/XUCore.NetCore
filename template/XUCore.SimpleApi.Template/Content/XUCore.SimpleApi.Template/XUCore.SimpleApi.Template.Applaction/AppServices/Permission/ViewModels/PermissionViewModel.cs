using System.Collections.Generic;
using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.SimpleApi.Template.Applaction.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
