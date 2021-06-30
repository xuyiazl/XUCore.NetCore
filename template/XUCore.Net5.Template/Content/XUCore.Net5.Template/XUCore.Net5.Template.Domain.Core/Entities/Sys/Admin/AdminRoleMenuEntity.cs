using XUCore.Ddd.Domain;

namespace XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin
{
    /// <summary>
    /// 角色导航关联表
    /// </summary>
    public partial class AdminRoleMenuEntity : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 导航id
        /// </summary>
        public long MenuId { get; set; }
        /// <summary>
        /// 对应关联的导航
        /// </summary>
        public AdminMenuEntity Menus { get; set; }
        /// <summary>
        /// 对应关联的角色
        /// </summary>
        public AdminRoleEntity Role { get; set; }
    }
}
