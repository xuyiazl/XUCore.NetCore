using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Ddd.Domain;

namespace XUCore.Template.Ddd.Domain.Core.Entities.Auth
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public partial class UserRoleEntity : BaseKeyEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 对应用户
        /// </summary>
        public UserEntity User { get; set; }
        /// <summary>
        /// 对应角色
        /// </summary>
        public RoleEntity Role { get; set; }
    }
}
