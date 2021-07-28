using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Diagnostics.CodeAnalysis;
using XUCore.Helpers;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Mapping
{
    public class TestKeyMap : BaseMapping<TestKeyEntity, string>
    {
        public TestKeyMap() : base("TestKey", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<TestKeyEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<PrimaryKeyIncrementGenerator>();
        }

        public class PrimaryKeyIncrementGenerator : ValueGenerator<string>
        {
            public override bool GeneratesTemporaryValues => false;

            public override string Next([NotNull] EntityEntry entry)
            {
                return Id.SequentialString.Binary;
            }
        }
    }
}
