﻿using XUCore.Ddd.Domain;

namespace XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin
{
    public partial class AdminUserRoleEntity : Entity<long>, IAggregateRoot
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public AdminUserEntity AdminUser { get; set; }
        public AdminRoleEntity Role { get; set; }
    }
}
