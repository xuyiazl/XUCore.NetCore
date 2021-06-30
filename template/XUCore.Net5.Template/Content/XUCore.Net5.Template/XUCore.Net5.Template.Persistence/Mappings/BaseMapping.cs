using XUCore.Net5.Template.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence.Mappings
{
    public abstract class BaseMapping<TEntity> : KeyMapping<TEntity, long>
         where TEntity : BaseEntity, new()
    {
        public BaseMapping(string tableName, Expression<Func<TEntity, object>> primaryKey) : base(tableName, primaryKey) { }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            #region [ 公共字段 ]

            builder.Property(e => e.Status)
                .HasColumnType("int")
                .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

            builder.Property(e => e.Created_At)
                .HasColumnType("datetime")
                .HasColumnName("Created_At")
                .HasComment("添加日期");

            builder.Property(e => e.Updated_At)
                .HasColumnType("datetime")
                .HasColumnName("Updated_At")
                .HasComment("最后修改日期");

            builder.Property(e => e.Deleted_At)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_At")
                .HasComment("删除日期");

            #endregion
        }
    }
}
