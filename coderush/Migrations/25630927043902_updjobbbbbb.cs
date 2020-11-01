using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updjobbbbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_DoctorType_DoctorTypeId",
                table: "Doctor");

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "MDLicense",
                table: "Doctor",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorTypeId",
                table: "Doctor",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    GenderId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.GenderId);
                    table.ForeignKey(
                        name: "FK_Gender_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gender_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gender_CreatedById",
                table: "Gender",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Gender_UpdatedById",
                table: "Gender",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_DoctorType_DoctorTypeId",
                table: "Doctor",
                column: "DoctorTypeId",
                principalTable: "DoctorType",
                principalColumn: "DoctorTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_DoctorType_DoctorTypeId",
                table: "Doctor");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Job");

            migrationBuilder.AlterColumn<string>(
                name: "MDLicense",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DoctorTypeId",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_DoctorType_DoctorTypeId",
                table: "Doctor",
                column: "DoctorTypeId",
                principalTable: "DoctorType",
                principalColumn: "DoctorTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
