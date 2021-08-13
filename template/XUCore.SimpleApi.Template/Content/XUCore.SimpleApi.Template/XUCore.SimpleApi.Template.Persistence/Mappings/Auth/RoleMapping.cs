using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Net5.Template.Persistence.Mappings.Auth
{
    public class RoleMapping : BaseMapping<RoleEntity>
    {
        public RoleMapping() : base("t_auth_role", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            
        }
    }
}
