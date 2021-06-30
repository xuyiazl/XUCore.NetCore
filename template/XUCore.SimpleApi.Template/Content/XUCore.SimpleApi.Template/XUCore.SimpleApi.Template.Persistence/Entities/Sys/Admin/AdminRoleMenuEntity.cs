using XUCore.Ddd.Domain;

namespace XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin
{
    public partial class AdminRoleMenuEntity : Entity<long>, IAggregateRoot
    {
        public long RoleId { get; set; }
        public long MenuId { get; set; }
        public AdminMenuEntity Menus { get; set; }
        public AdminRoleEntity Role { get; set; }
    }
}
