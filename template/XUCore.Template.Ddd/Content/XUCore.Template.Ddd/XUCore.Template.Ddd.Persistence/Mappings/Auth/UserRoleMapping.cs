using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings.Auth
{
    public class UserRoleMapping : BaseKeyMapping<UserRoleEntity>
    {
        public UserRoleMapping() : base("t_auth_userrole", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.HasOne(d => d.User)
              .WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_User_UserRole");


            builder.HasOne(d => d.Role)
              .WithMany(p => p.UserRoles)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Role_UserRole");
        }
    }
}
