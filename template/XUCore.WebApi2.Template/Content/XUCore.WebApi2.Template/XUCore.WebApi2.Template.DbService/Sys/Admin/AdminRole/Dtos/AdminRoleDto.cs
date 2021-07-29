using XUCore.WebApi2.Template.Core;
using XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi2.Template.DbService.Sys.Admin.AdminRole
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
