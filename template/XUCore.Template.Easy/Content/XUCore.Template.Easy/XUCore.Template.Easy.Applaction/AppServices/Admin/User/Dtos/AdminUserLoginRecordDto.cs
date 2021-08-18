using System;
using XUCore.Template.Easy.Core;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 登录记录
    /// </summary>
    public class AdminUserLoginRecordDto : DtoBase<AdminUserLoginRecordViewModel>
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
        /// 管理员名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 管理员手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 管理员头像
        /// </summary>
        public string Picture { get; set; }
    }
}
