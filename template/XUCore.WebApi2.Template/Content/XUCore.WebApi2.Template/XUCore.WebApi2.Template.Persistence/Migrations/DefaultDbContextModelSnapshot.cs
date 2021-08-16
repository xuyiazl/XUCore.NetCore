﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XUCore.WebApi2.Template.Persistence;

namespace XUCore.WebApi2.Template.Persistence.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    partial class DefaultDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminLoginRecordEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AdminId")
                        .HasColumnType("bigint")
                        .HasColumnName("AdminId");

                    b.Property<string>("LoginIp")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("LoginIP")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("LoginTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("LoginWay")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasCharSet("utf8");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("sys_admin_loginrecord");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminMenuEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt")
                        .HasComment("添加日期");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("DeletedAt")
                        .HasComment("删除日期");

                    b.Property<long>("FatherId")
                        .HasColumnType("bigint")
                        .HasColumnName("FatherID");

                    b.Property<string>("Icon")
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<ulong>("IsExpress")
                        .HasColumnType("bit(1)");

                    b.Property<ulong>("IsMenu")
                        .HasColumnType("bit(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<string>("OnlyCode")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedAt")
                        .HasComment("最后修改日期");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("sys_admin_authmenus");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt")
                        .HasComment("添加日期");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("DeletedAt")
                        .HasComment("删除日期");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedAt")
                        .HasComment("最后修改日期");

                    b.HasKey("Id");

                    b.ToTable("sys_admin_authrole");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleMenuEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("MenuId")
                        .HasColumnType("bigint")
                        .HasColumnName("MenuID");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("RoleID");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.HasIndex("RoleId");

                    b.ToTable("sys_admin_authrolemenus");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt")
                        .HasComment("添加日期");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("DeletedAt")
                        .HasComment("删除日期");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<int>("LoginCount")
                        .HasColumnType("int");

                    b.Property<string>("LoginLastIp")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("LoginLastIP")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("LoginLastTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<string>("Picture")
                        .HasColumnType("varchar(250)")
                        .HasCharSet("utf8");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedAt")
                        .HasComment("最后修改日期");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.HasKey("Id");

                    b.ToTable("sys_admin_users");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserRoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("AdminId")
                        .HasColumnType("bigint")
                        .HasColumnName("UserID");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("RoleID");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("RoleId");

                    b.ToTable("sys_admin_authuserrole");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminLoginRecordEntity", b =>
                {
                    b.HasOne("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserEntity", "AdminUser")
                        .WithMany("LoginRecords")
                        .HasForeignKey("AdminId")
                        .HasConstraintName("FK_AdminUser_AdminLoginRecord")
                        .IsRequired();

                    b.Navigation("AdminUser");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleMenuEntity", b =>
                {
                    b.HasOne("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminMenuEntity", "Menus")
                        .WithMany("RoleMenus")
                        .HasForeignKey("MenuId")
                        .HasConstraintName("FK_AdminAuthMenus_AdminAuthRoleMenus")
                        .IsRequired();

                    b.HasOne("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleEntity", "Role")
                        .WithMany("RoleMenus")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AdminAuthRole_AdminAuthRoleMenus")
                        .IsRequired();

                    b.Navigation("Menus");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserRoleEntity", b =>
                {
                    b.HasOne("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserEntity", "AdminUser")
                        .WithMany("UserRoles")
                        .HasForeignKey("AdminId")
                        .HasConstraintName("FK_AdminUser_AdminAuthUserRole")
                        .IsRequired();

                    b.HasOne("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AdminAuthRole_AdminAuthUserRoles")
                        .IsRequired();

                    b.Navigation("AdminUser");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminMenuEntity", b =>
                {
                    b.Navigation("RoleMenus");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminRoleEntity", b =>
                {
                    b.Navigation("RoleMenus");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin.AdminUserEntity", b =>
                {
                    b.Navigation("LoginRecords");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
