using System;
using XUCore.Ddd.Domain;

namespace XUCore.Template.Ddd.Domain.Core.Entities.User
{
    /// <summary>
    /// 登录记录表
    /// </summary>
    public partial class UserLoginRecordEntity : BaseKeyEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
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
        /// 对应用户
        /// </summary>
        public UserEntity User { get; set; }
    }
}
