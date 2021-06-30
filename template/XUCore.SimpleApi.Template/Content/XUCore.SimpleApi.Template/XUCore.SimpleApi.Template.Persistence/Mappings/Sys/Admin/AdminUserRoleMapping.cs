using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace XUCore.SimpleApi.Template.Persistence.Mappings.Sys.Admin
{
    public class AdminUserRoleMapping : KeyMapping<AdminUserRoleEntity, long>
    {
        public AdminUserRoleMapping() : base("sys_admin_authuserrole", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<AdminUserRoleEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("RoleID");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("UserID");

            builder.HasOne(d => d.AdminUser)
              .WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminUser_AdminAuthUserRole");


            builder.HasOne(d => d.Role)
              .WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminAuthRole_AdminAuthUserRoles");
        }
    }
}
