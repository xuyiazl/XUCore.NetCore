using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings.Auth
{
    public class MenuMapping : BaseMapping<MenuEntity>
    {
        public MenuMapping() : base("t_auth_menu", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<MenuEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.FatherId)
                .IsRequired()
                .HasColumnType("varchar(50)");

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

            builder.Property(e => e.Weight).HasColumnType("int");

            
        }
    }
}
