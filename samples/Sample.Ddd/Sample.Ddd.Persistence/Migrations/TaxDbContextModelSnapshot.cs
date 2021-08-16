﻿// <auto-generated />
using System;
using Sample.Ddd.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sample.Ddd.Persistence.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    partial class TaxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.MenuEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasComment("创建时间");

                    b.Property<string>("CreatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("创建人");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasComment("删除日期");

                    b.Property<string>("DeletedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("删除人");

                    b.Property<string>("FatherId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

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
                        .HasComment("最后修改日期");

                    b.Property<string>("UpdatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("最后修改人");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<int>("Weight")
                        .HasColumnType("int(11)");

                    b.HasKey("Id");

                    b.ToTable("t_auth_menu");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.RoleEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasComment("创建时间");

                    b.Property<string>("CreatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("创建人");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasComment("删除日期");

                    b.Property<string>("DeletedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("删除人");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasCharSet("utf8");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("数据状态（1、正常 2、不显示 3、已删除）");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasComment("最后修改日期");

                    b.Property<string>("UpdatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("最后修改人");

                    b.HasKey("Id");

                    b.ToTable("t_auth_role");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.RoleMenuEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<string>("MenuId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.HasIndex("RoleId");

                    b.ToTable("t_auth_rolemenu");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.UserRoleEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("t_auth_userrole");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.User.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasComment("创建时间");

                    b.Property<string>("CreatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("创建人");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasComment("删除日期");

                    b.Property<string>("DeletedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("删除人");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<int>("LoginCount")
                        .HasColumnType("int(11)");

                    b.Property<string>("LoginLastIp")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
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
                        .HasComment("最后修改日期");

                    b.Property<string>("UpdatedAtUserId")
                        .HasColumnType("varchar(50)")
                        .HasComment("最后修改人");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.HasKey("Id");

                    b.ToTable("t_user");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.User.UserLoginRecordEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<string>("LoginIp")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("LoginTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("LoginWay")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasCharSet("utf8");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("t_user_loginrecord");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Events.StoredEvent", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(50)")
                        .HasComment("主键Id");

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasCharSet("utf8");

                    b.HasKey("Id");

                    b.ToTable("t_stored_event");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.RoleMenuEntity", b =>
                {
                    b.HasOne("Sample.Ddd.Domain.Core.Entities.Auth.MenuEntity", "Menu")
                        .WithMany("RoleMenus")
                        .HasForeignKey("MenuId")
                        .HasConstraintName("FK_Menu_RoleMenu")
                        .IsRequired();

                    b.HasOne("Sample.Ddd.Domain.Core.Entities.Auth.RoleEntity", "Role")
                        .WithMany("RoleMenus")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Role_RoleMenu")
                        .IsRequired();

                    b.Navigation("Menu");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.UserRoleEntity", b =>
                {
                    b.HasOne("Sample.Ddd.Domain.Core.Entities.Auth.RoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Role_UserRole")
                        .IsRequired();

                    b.HasOne("Sample.Ddd.Domain.Core.Entities.User.UserEntity", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_User_UserRole")
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.User.UserLoginRecordEntity", b =>
                {
                    b.HasOne("Sample.Ddd.Domain.Core.Entities.User.UserEntity", "User")
                        .WithMany("LoginRecords")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_User_UserLoginRecord")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.MenuEntity", b =>
                {
                    b.Navigation("RoleMenus");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.Auth.RoleEntity", b =>
                {
                    b.Navigation("RoleMenus");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Sample.Ddd.Domain.Core.Entities.User.UserEntity", b =>
                {
                    b.Navigation("LoginRecords");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}