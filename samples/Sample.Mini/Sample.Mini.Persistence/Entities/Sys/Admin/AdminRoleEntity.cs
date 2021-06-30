
using System;
using System.Collections.Generic;

namespace Sample.Mini.Persistence.Entities.Sys.Admin
{
    public partial class AdminRoleEntity : BaseEntity
    {
        public AdminRoleEntity()
        {
            RoleMenus = new HashSet<AdminRoleMenuEntity>();
            UserRoles = new HashSet<AdminUserRoleEntity>();
        }

        public string Name { get; set; }

        public ICollection<AdminRoleMenuEntity> RoleMenus;

        public ICollection<AdminUserRoleEntity> UserRoles;
    }
}
