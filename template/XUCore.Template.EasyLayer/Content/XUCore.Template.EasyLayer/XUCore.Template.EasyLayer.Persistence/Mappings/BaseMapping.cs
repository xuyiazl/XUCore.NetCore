﻿using XUCore.Template.EasyLayer.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;
using XUCore.NetCore.Data;

namespace XUCore.Template.EasyLayer.Persistence.Mappings
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
                .HasComment("添加日期");

            builder.Property(e => e.Updated_At)
                .HasColumnType("datetime")
                .HasComment("最后修改日期");

            builder.Property(e => e.Deleted_At)
                .HasColumnType("datetime")
                .HasComment("删除日期");

            #endregion
        }
    }
}