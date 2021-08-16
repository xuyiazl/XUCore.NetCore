using XUCore.Template.Ddd.Domain.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings.User
{
    public class UserLoginRecordMapping : BaseKeyMapping<UserLoginRecordEntity>
    {
        public UserLoginRecordMapping() : base("t_user_loginrecord", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<UserLoginRecordEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(e => e.LoginIp)
                .IsRequired()
                .HasColumnType("varchar(50)")
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

            builder.HasOne(d => d.User)
              .WithMany(p => p.LoginRecords)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_User_UserLoginRecord");
        }
    }
}
