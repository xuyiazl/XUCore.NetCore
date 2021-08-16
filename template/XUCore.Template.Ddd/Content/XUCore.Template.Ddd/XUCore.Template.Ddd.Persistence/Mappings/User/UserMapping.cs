using XUCore.Template.Ddd.Domain.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings.User
{
    public class UserMapping : BaseMapping<UserEntity>
    {
        public UserMapping() : base("t_user", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<UserEntity> builder)
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
                .HasColumnType("int");

            builder.Property(e => e.LoginLastIp)
                .IsRequired()
                .HasColumnType("varchar(50)")
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
