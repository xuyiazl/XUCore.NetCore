using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using System;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    public class UserLoginRecordDto : DtoKeyBase<UserLoginRecordViewModel>
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
