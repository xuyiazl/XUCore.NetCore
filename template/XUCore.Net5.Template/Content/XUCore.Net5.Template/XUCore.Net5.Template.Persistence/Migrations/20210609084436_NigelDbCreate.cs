using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XUCore.Net5.Template.Persistence.Migrations
{
    public partial class NigelDbCreate : Migration
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
                    FatherID = table.Column<long>(type: "bigint(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Icon = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    Url = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    OnlyCode = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    IsMenu = table.Column<ulong>(type: "bit(1)", nullable: false),
                    Weight = table.Column<int>(type: "int(11)", nullable: false),
                    IsExpress = table.Column<ulong>(type: "bit(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    Updated_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    Deleted_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
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
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    Updated_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    Deleted_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
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
                    LoginCount = table.Column<int>(type: "int(11)", nullable: false),
                    LoginLastTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginLastIP = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加日期"),
                    Updated_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    Deleted_At = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期")
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
                    RoleID = table.Column<long>(type: "bigint(20)", nullable: false),
                    MenuID = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authrolemenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAuthMenus_AdminAuthRoleMenus",
                        column: x => x.MenuID,
                        principalTable: "sys_admin_authmenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminAuthRole_AdminAuthRoleMenus",
                        column: x => x.RoleID,
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
                    UserID = table.Column<long>(type: "bigint(20)", nullable: false),
                    RoleID = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_admin_authuserrole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAuthRole_AdminAuthUserRoles",
                        column: x => x.RoleID,
                        principalTable: "sys_admin_authrole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminAuthUserRole",
                        column: x => x.UserID,
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
                    AdminId = table.Column<long>(type: "bigint(20)", nullable: false),
                    LoginWay = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    LoginTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LoginIP = table.Column<string>(type: "varchar(50)", nullable: false)
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
                name: "IX_sys_admin_authrolemenus_MenuID",
                table: "sys_admin_authrolemenus",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authrolemenus_RoleID",
                table: "sys_admin_authrolemenus",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authuserrole_RoleID",
                table: "sys_admin_authuserrole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_sys_admin_authuserrole_UserID",
                table: "sys_admin_authuserrole",
                column: "UserID");

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
