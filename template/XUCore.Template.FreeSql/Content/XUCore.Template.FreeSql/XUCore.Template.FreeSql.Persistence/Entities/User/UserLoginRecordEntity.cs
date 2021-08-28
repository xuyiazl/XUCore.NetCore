using FreeSql.DataAnnotations;
using System;
using XUCore.Ddd.Domain;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 登录记录表
    /// </summary>
	[Table(Name = "sys_user_login_log")]
    public partial class UserLoginRecordEntity : EntityAdd
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        [Column(StringLength = 30)]
        public string LoginWay { get; set; }
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
