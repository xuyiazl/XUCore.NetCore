using XUCore.Ddd.Domain;

namespace XUCore.Template.Ddd.Domain.Core.Entities.Auth
{
    /// <summary>
    /// 角色导航关联表
    /// </summary>
    public partial class RoleMenuEntity : BaseKeyEntity
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 导航id
        /// </summary>
        public string MenuId { get; set; }
        /// <summary>
        /// 对应关联的导航
        /// </summary>
        public MenuEntity Menu { get; set; }
        /// <summary>
        /// 对应关联的角色
        /// </summary>
        public RoleEntity Role { get; set; }
    }
}
