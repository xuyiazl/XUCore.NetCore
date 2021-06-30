using XUCore.WebApi.Template.Persistence.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.WebApi.Template.Persistence.Mappings.Sys.Admin
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
