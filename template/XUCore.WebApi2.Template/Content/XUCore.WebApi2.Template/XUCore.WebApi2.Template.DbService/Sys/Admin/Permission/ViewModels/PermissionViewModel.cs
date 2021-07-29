using System.Collections.Generic;
using XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi2.Template.DbService.Sys.Admin.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
