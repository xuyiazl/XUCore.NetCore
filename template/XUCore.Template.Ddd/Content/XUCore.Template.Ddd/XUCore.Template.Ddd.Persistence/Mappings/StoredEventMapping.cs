using XUCore.Template.Ddd.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XUCore.Template.Ddd.Persistence.Mappings
{
    public class StoredEventMapping : BaseKeyMapping<StoredEvent>
    {
        public StoredEventMapping() : base("t_stored_event", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            base.Configure(builder);


            builder.Property(e => e.MessageType)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.AggregateId)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");

            builder.Property(e => e.Data)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(e => e.Timestamp).HasColumnType("datetime");
        }
    }
}
