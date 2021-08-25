using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.FreeSql.DbService.Sys.Admin.AdminRole
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
