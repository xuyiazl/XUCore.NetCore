using WebApi.Core;
using WebApi.Persistence.Entities.Sys.Admin;

namespace WebApi.DbService.Sys.Admin.AdminRole
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
