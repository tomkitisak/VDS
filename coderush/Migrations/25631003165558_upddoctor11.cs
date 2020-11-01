using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class upddoctor11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserTypeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    UserTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedById1 = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedById1 = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.UserTypeId);
                    table.ForeignKey(
                        name: "FK_UserType_AspNetUsers_CreatedById1",
                        column: x => x.CreatedById1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserType_AspNetUsers_UpdatedById1",
                        column: x => x.UpdatedById1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserType_CreatedById1",
                table: "UserType",
                column: "CreatedById1");

            migrationBuilder.CreateIndex(
                name: "IX_UserType_UpdatedById1",
                table: "UserType",
                column: "UpdatedById1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "AspNetUsers");
        }
    }
}
