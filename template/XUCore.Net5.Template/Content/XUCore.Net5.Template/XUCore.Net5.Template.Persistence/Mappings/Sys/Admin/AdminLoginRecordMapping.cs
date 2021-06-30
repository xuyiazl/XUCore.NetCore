using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings.Sys.Admin
{
    public class AdminLoginRecordMapping : KeyMapping<LoginRecordEntity, long>
    {
        public AdminLoginRecordMapping() : base("sys_admin_loginrecord", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<LoginRecordEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AdminId)
                .IsRequired()
                .HasColumnType("bigint(20)")
                .HasColumnName("AdminId");

            builder.Property(e => e.LoginIp)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("LoginIP")
                .HasCharSet("utf8");

            builder.Property(e => e.LoginTime)
                .IsRequired()
                .HasColumnType("datetime")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.LoginWay)
                .IsRequired()
                .HasColumnType("varchar(30)")
                .HasCharSet("utf8");

            builder.HasOne(d => d.AdminUser)
              .WithMany(p => p.LoginRecords)
              .HasForeignKey(d => d.AdminId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminUser_AdminLoginRecord");
        }
    }
}
