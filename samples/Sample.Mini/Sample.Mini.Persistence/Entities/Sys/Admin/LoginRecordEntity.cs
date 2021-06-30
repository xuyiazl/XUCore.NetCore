using System;
using XUCore.Ddd.Domain;

namespace Sample.Mini.Persistence.Entities.Sys.Admin
{
    public partial class LoginRecordEntity : Entity<long>, IAggregateRoot
    {
        public long AdminId { get; set; }
        public string LoginWay { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginIp { get; set; }
        public AdminUserEntity AdminUser { get; set; }
    }
}
