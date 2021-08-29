using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.AdminUser
{
    /// <summary>
    /// 管理员信息
    /// </summary>
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
    }
}
