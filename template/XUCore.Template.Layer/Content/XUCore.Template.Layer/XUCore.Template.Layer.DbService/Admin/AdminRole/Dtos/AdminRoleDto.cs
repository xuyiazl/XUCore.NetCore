using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.AdminRole
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
