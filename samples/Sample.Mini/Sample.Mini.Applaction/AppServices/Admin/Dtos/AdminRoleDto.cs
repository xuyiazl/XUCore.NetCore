using Sample.Mini.Persistence.Entities.Sys.Admin;
using Sample.Mini.Core;

namespace Sample.Mini.Applaction.Admin
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
