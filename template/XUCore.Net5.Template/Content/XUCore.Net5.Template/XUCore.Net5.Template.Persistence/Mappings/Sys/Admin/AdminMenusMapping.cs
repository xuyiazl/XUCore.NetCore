using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings.Sys.Admin
{
    public class AdminMenusMapping : BaseMapping<AdminMenuEntity>
    {
        public AdminMenusMapping() : base("sys_admin_authmenus", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<AdminMenuEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.FatherId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("FatherID");

            builder.Property(e => e.Icon)
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            builder.Property(e => e.IsExpress)
                .IsRequired()
                .HasColumnType("bit(1)");

            builder.Property(e => e.IsMenu)
                .IsRequired()
                .HasColumnType("bit(1)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            builder.Property(e => e.OnlyCode)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            builder.Property(e => e.Url)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8");

            builder.Property(e => e.Weight).HasColumnType("int(11)");

            
        }
    }
}
