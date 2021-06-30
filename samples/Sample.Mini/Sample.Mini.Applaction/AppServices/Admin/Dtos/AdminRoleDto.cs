using Sample.Mini.Persistence.Entities.Sys.Admin;

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
