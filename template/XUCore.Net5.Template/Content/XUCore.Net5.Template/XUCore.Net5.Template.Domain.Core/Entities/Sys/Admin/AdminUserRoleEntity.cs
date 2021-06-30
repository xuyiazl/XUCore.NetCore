using XUCore.Ddd.Domain;

namespace XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin
{
    /// <summary>
    /// 管理员角色关联表
    /// </summary>
    public partial class AdminUserRoleEntity : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        public long AdminId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 对应管理员
        /// </summary>
        public AdminUserEntity AdminUser { get; set; }
        /// <summary>
        /// 对应角色
        /// </summary>
        public AdminRoleEntity Role { get; set; }
    }
}
