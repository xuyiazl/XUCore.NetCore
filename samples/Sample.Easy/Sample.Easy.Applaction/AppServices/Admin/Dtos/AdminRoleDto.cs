using Sample.Easy.Core;
using Sample.Easy.Persistence.Entities.Sys.Admin;

namespace Sample.Easy.Applaction.Admin
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
