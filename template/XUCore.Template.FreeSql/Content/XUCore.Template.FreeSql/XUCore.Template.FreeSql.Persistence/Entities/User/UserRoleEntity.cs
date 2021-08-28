using FreeSql.DataAnnotations;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
	[Table(Name = "sys_user_role")]
    [Index("idx_{tablename}_01", nameof(UserId) + "," + nameof(RoleId), true)]
    public partial class UserRoleEntity : EntityAdd
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }
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
