
using System;
using System.Collections.Generic;

namespace Sample.Plain.Persistence.Entities.Sys.Admin
{
    public partial class AdminUserEntity : BaseEntity
    {
        public AdminUserEntity()
        {
            UserRoles = new HashSet<AdminUserRoleEntity>();
            LoginRecords = new HashSet<LoginRecordEntity>();
        }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int LoginCount { get; set; }
        public DateTime LoginLastTime { get; set; }
        public string LoginLastIp { get; set; }

        public ICollection<AdminUserRoleEntity> UserRoles;
        public ICollection<LoginRecordEntity> LoginRecords;
    }
}
