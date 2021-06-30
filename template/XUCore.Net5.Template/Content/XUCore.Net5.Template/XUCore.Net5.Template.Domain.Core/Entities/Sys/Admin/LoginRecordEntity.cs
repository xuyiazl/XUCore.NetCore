using System;
using XUCore.Ddd.Domain;

namespace XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin
{
    /// <summary>
    /// 登录记录表
    /// </summary>
    public partial class LoginRecordEntity : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        public long AdminId { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        public string LoginWay { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        public string LoginIp { get; set; }
        /// <summary>
        /// 对应管理员
        /// </summary>
        public AdminUserEntity AdminUser { get; set; }
    }
}
