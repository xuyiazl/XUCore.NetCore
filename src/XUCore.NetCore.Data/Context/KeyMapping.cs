using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;
using XUCore.Ddd.Domain;

namespace XUCore.NetCore.Data
{
    public abstract class KeyMapping<TEntity, TKey> : EntityTypeConfiguration<TEntity>
         where TEntity : Entity<TKey>, new()
    {
        public KeyMapping(string tableName, Expression<Func<TEntity, object>> primaryKey) : base(tableName, primaryKey)
        {
            SetIndentity(t => t.Id);
        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
