using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings.Sys.Admin
{
    public class AdminUserMapping : BaseMapping<AdminUserEntity>
    {
        public AdminUserMapping() : base("sys_admin_users", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<AdminUserEntity> builder)
        {
            base.Configure(builder);


            builder.Property(e => e.Company)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.Location)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.LoginCount)
                .IsRequired()
                .HasColumnType("int(11)");

            builder.Property(e => e.LoginLastIp)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("LoginLastIP")
                .HasCharSet("utf8");

            builder.Property(e => e.LoginLastTime).HasColumnType("datetime");

            builder.Property(e => e.Mobile)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.Picture)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");

            builder.Property(e => e.Position)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            
        }
    }
}
