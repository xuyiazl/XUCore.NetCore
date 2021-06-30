using System.Collections.Generic;
using Sample.Plain.Persistence.Entities.Sys.Admin;

namespace Sample.Plain.DbService.Sys.Admin.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
