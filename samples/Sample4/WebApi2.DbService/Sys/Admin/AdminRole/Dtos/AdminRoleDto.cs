using WebApi2.Core;
using WebApi2.Persistence.Entities.Sys.Admin;

namespace WebApi2.DbService.Sys.Admin.AdminRole
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
