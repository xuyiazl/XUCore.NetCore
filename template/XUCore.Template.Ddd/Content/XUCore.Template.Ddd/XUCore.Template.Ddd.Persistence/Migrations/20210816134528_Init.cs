using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XUCore.Template.Ddd.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_auth_menu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FatherId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    CreatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    UpdatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期"),
                    DeletedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "删除人")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_auth_menu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_auth_role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "数据状态（1、正常 2、不显示 3、已删除）"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    CreatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    UpdatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期"),
                    DeletedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "删除人")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_auth_role", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_stored_event",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    AggregateId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Data = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_stored_event", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    CreatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后修改日期"),
                    UpdatedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "删除日期"),
                    DeletedAtUserId = table.Column<string>(type: "varchar(50)", nullable: true, comment: "删除人")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_auth_rolemenu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_auth_rolemenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_RoleMenu",
                        column: x => x.MenuId,
                        principalTable: "t_auth_menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_RoleMenu",
                        column: x => x.RoleId,
                        principalTable: "t_auth_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_auth_userrole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_auth_userrole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_UserRole",
                        column: x => x.RoleId,
                        principalTable: "t_auth_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_UserRole",
                        column: x => x.UserId,
                        principalTable: "t_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_user_loginrecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, comment: "主键Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginWay = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    LoginTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LoginIp = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_loginrecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserLoginRecord",
                        column: x => x.UserId,
                        principalTable: "t_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_t_auth_rolemenu_MenuId",
                table: "t_auth_rolemenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_t_auth_rolemenu_RoleId",
                table: "t_auth_rolemenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_t_auth_userrole_RoleId",
                table: "t_auth_userrole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_t_auth_userrole_UserId",
                table: "t_auth_userrole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_loginrecord_UserId",
                table: "t_user_loginrecord",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_auth_rolemenu");

            migrationBuilder.DropTable(
                name: "t_auth_userrole");

            migrationBuilder.DropTable(
                name: "t_stored_event");

            migrationBuilder.DropTable(
                name: "t_user_loginrecord");

            migrationBuilder.DropTable(
                name: "t_auth_menu");

            migrationBuilder.DropTable(
                name: "t_auth_role");

            migrationBuilder.DropTable(
                name: "t_user");
        }
    }
}
