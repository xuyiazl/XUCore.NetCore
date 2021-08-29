using XUCore.Template.Easy.Persistence.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Easy.Persistence.Mappings.Admin
{
    public class AdminRoleMapping : BaseMapping<AdminRoleEntity>
    {
        public AdminRoleMapping() : base("sys_admin_authrole", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<AdminRoleEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            
        }
    }
}
