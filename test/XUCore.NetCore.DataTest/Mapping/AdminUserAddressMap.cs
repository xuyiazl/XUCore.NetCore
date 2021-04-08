using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Mapping
{
    public class AdminUserAddressMap : EntityTypeConfiguration<AdminUserAddressEntity>
    {
        public AdminUserAddressMap() : base("AdminUserAddress", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }

        public override void Configure(EntityTypeBuilder<AdminUserAddressEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(d => d.AdminUser)
                .WithMany(p => p.AdminUserAddress)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminUserAddress_AdminUser");
        }
    }
}
