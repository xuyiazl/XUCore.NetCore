using FreeSql.DataAnnotations;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 角色导航关联表
    /// </summary>
	[Table(Name = "sys_role_menu")]
    [Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(MenuId), true)]
    public partial class RoleMenuEntity : EntityAdd
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
        public MenuEntity Menus { get; set; }
        /// <summary>
        /// 对应关联的角色
        /// </summary>
        public RoleEntity Role { get; set; }
    }
}
