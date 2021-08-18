using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
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
