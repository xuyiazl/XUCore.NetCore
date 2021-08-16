using XUCore.Template.Ddd.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using XUCore.Helpers;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Persistence.Mappings
{
    public abstract class BaseMapping<TEntity> : KeyMapping<TEntity, string>
         where TEntity : BaseEntity, new()
    {
        public BaseMapping(string tableName, Expression<Func<TEntity, object>> primaryKey) : base(tableName, primaryKey) { }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Id)
                .HasColumnType("varchar(50)")
                .HasComment("主键Id")
                .ValueGeneratedOnAdd()
                .HasValueGenerator<PrimaryKeyIncrementGenerator>();

            builder.Property(e => e.Status)
                .HasColumnType("int")
                .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasComment("创建时间");

            builder.Property(e => e.CreatedAtUserId)
                .HasColumnType("varchar(50)")
                .HasComment("创建人");

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasComment("最后修改日期");

            builder.Property(e => e.UpdatedAtUserId)
                .HasColumnType("varchar(50)")
                .HasComment("最后修改人");

            builder.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasComment("删除日期");

            builder.Property(e => e.DeletedAtUserId)
                .HasColumnType("varchar(50)")
                .HasComment("删除人");
        }
    }

    public abstract class BaseKeyMapping<TEntity> : KeyMapping<TEntity, string>
         where TEntity : BaseKeyEntity, new()
    {
        public BaseKeyMapping(string tableName, Expression<Func<TEntity, object>> primaryKey) : base(tableName, primaryKey) { }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder
                .Property(c => c.Id)
                .HasColumnType("varchar(50)")
                .HasComment("主键Id")
                .ValueGeneratedOnAdd()
                .HasValueGenerator<PrimaryKeyIncrementGenerator>();

        }
    }

    public class PrimaryKeyIncrementGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next([NotNull] EntityEntry entry)
        {
            return Id.SequentialString.String;
        }
    }
}
