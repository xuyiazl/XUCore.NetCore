using System.Collections.Generic;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
