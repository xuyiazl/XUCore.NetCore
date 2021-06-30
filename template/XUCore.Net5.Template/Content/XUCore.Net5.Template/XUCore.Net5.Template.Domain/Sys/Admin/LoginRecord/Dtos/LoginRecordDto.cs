using AutoMapper;
using XUCore.Net5.Template.Domain.Common.Mappings;
using System;

namespace XUCore.Net5.Template.Domain.Sys.LoginRecord
{
    public class LoginRecordDto : DtoBase<LoginRecordViewModel>
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
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
    }
}
