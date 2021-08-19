using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XUCore.Template.Layer.Persistence.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_authmenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FatherId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Icon = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    Url = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    OnlyCode = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    IsMenu = table.Column<ulong>(type: "bit(1)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    IsExpress = table.Column<ulong>(type: "bit(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authmenus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_authrole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authrole", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Mobile = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Password = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Picture = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    Location = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Position = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Company = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    LoginLastTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginLastIp = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_authrolemenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authrolemenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAuthMenus_AdminAuthRoleMenus",
                        column: x => x.MenuId,
                        principalTable: "sys_admin_authmenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminAuthRole_AdminAuthRoleMenus",
                        column: x => x.RoleId,
                        principalTable: "sys_admin_authrole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_authuserrole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authuserrole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAuthRole_AdminAuthUserRoles",
                        column: x => x.RoleId,
                        principalTable: "sys_admin_authrole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminAuthUserRole",
                        column: x => x.AdminId,
                        principalTable: "sys_admin_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_admin_loginrecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    LoginWay = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    LoginTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LoginIp = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_loginrecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminLoginRecord",
                        column: x => x.AdminId,
                        principalTable: "sys_admin_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authrolemenus_MenuId",
                table: "sys_admin_authrolemenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authrolemenus_RoleId",
                table: "sys_admin_authrolemenus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authuserrole_AdminId",
                table: "sys_admin_authuserrole",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authuserrole_RoleId",
                table: "sys_admin_authuserrole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_loginrecord_AdminId",
                table: "sys_admin_loginrecord",
                column: "AdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_admin_authrolemenus");

            migrationBuilder.DropTable(
                name: "sys_admin_authuserrole");

            migrationBuilder.DropTable(
                name: "sys_admin_loginrecord");

            migrationBuilder.DropTable(
                name: "sys_admin_authmenus");

            migrationBuilder.DropTable(
                name: "sys_admin_authrole");

            migrationBuilder.DropTable(
                name: "sys_admin_users");
        }
    }
}
