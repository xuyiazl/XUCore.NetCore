using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings.Sys.Admin
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
