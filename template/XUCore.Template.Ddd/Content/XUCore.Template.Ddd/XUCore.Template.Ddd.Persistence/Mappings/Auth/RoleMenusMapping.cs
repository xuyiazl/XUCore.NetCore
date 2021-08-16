using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings.Auth
{
    public class RoleMenusMapping : BaseKeyMapping<RoleMenuEntity>
    {
        public RoleMenusMapping() : base("t_auth_rolemenu", t => t.Id)
        {

        }
        public override void Configure(EntityTypeBuilder<RoleMenuEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MenuId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.HasOne(d => d.Role)
              .WithMany(p => p.RoleMenus)
              .HasForeignKey(d => d.RoleId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Role_RoleMenu");


            builder.HasOne(d => d.Menu)
              .WithMany(p => p.RoleMenus)
              .HasForeignKey(d => d.MenuId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Menu_RoleMenu");
        }
    }
}
