using AutoMapper;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    public class AdminUserDto : DtoBase<AdminUserEntity>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginLastTime { get; set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LoginLastIp { get; set; }
    }
}
