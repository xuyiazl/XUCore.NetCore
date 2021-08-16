﻿using Sample.Easy.Persistence.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data;

namespace Sample.Easy.Persistence.Mappings.Sys.Admin
{
    public class AdminLoginRecordMapping : KeyMapping<LoginRecordEntity, long>
    {
        public AdminLoginRecordMapping() : base("sys_admin_loginrecord", t => t.Id)
        {

        }

        public override void Configure(EntityTypeBuilder<LoginRecordEntity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AdminId)
                .IsRequired()
                .HasColumnType("bigint");

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

            builder.HasOne(d => d.AdminUser)
              .WithMany(p => p.LoginRecords)
              .HasForeignKey(d => d.AdminId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminUser_AdminLoginRecord");
        }
    }
}