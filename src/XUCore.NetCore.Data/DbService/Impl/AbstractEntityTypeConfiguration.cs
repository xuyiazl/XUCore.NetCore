using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// net core 跟 netFramework下面的EF统一配置字段不一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class, new()
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string tableName { get; set; }
        /// <summary>
        /// 设置主键
        /// </summary>
        private Expression<Func<T, object>> primaryKey { get; set; }

        /// <summary>
        /// 设置自增
        /// </summary>
        private Expression<Func<T, object>> Identity { get; set; }
        /// <summary>
        /// ef设置不自增
        /// </summary>
        private Expression<Func<T, object>> NoIdentity { get; set; }
        /// <summary>
        /// 需要排除的字段
        /// </summary>
        private Expression<Func<T, object>>[] ignores { get; set; }
        /// <summary>
        /// 仅在添加时设置指定字段默认值
        /// </summary>
        private Expression<Func<T, object>>[] insertDefault { get; set; }
        /// <summary>
        /// 仅在添加或者修改时设置默认值
        /// </summary>
        private Expression<Func<T, object>>[] insertOrUpdates { get; set; }

        public AbstractEntityTypeConfiguration() { }


        public AbstractEntityTypeConfiguration(string tableName, Expression<Func<T, object>> primaryKey)
        {
            this.tableName = tableName;
            this.primaryKey = primaryKey;
        }
        /// <summary>
        /// 设置自增属性
        /// </summary>
        /// <param name="Identity"></param>
        protected void SetIndentity(Expression<Func<T, object>> Identity)
        {
            this.Identity = Identity;
        }
        /// <summary>
        /// ef设置不自增
        /// </summary>
        /// <param name="NoIdentity"></param>
        protected void SetNoIndentity(Expression<Func<T, object>> NoIdentity)
        {
            this.NoIdentity = NoIdentity;
        }
        /// <summary>
        ///在ef映射过程中排除的字段,允许多个字段排除
        /// </summary>
        protected void SetIgnore(params Expression<Func<T, object>>[] ignores)
        {
            this.ignores = ignores;
        }
        /// <summary>
        /// 在ef添加记录的时候默认值添加，即不赋值
        /// </summary>
        /// <param name="insertDefault"></param>
        protected void SetValueDefaultAdd(params Expression<Func<T, object>>[] insertDefault)
        {
            this.insertDefault = insertDefault;
        }
        /// <summary>
        /// 仅在添加或者修改时设置默认值
        /// </summary>
        /// <param name="insertOrUpdates"></param>
        protected void SetValueInsertOrUpdate(params Expression<Func<T, object>>[] insertOrUpdates)
        {
            this.insertOrUpdates = insertOrUpdates;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(tableName);
            if (primaryKey != null)
            {
                //设置主键
                builder.HasKey(primaryKey);
            }
            if (Identity != null)
            {
                builder.Property(Identity).ValueGeneratedOnAdd();
            }

            if (NoIdentity != null)
            {
                builder.Property(NoIdentity).ValueGeneratedNever();
            }

            if (ignores != null)
            {
                foreach (var ignore in ignores)
                {
                    builder.Ignore(ignore);
                }
            }

            if (insertDefault != null)
            {
                foreach (var insert in insertDefault)
                {
                    builder.Property(insert).ValueGeneratedOnAdd();
                }
            }

            if (insertOrUpdates != null)
            {
                foreach (var insertOrUpdate in insertOrUpdates)
                {
                    builder.Property(insertOrUpdate).ValueGeneratedOnAddOrUpdate();
                }
            }
        }
    }
}
