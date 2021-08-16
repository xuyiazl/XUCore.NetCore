using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminRole
{
    /// <summary>
    /// 角色
    /// </summary>
    public class AdminRoleDto : DtoBase<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
