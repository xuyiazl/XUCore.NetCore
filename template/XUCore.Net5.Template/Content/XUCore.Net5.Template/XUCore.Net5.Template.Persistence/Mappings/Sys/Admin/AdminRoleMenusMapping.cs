using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings.Sys.Admin
{
    public class AdminRoleMenusMapping : KeyMapping<AdminRoleMenuEntity, long>
    {
        public AdminRoleMenusMapping() : base("sys_admin_authrolemenus", t => t.Id)
        {

        }
        public override void Configure(EntityTypeBuilder<AdminRoleMenuEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MenuId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("MenuID");

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("RoleID");

            builder.HasOne(d => d.Role)
              .WithMany(p => p.RoleMenus)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminAuthRole_AdminAuthRoleMenus");


            builder.HasOne(d => d.Menus)
              .WithMany(p => p.RoleMenus)
              .HasForeignKey(d => d.MenuId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminAuthMenus_AdminAuthRoleMenus");
        }
    }
}
