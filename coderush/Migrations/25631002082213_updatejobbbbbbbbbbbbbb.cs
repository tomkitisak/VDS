using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updatejobbbbbbbbbbbbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkStart",
                table: "Job",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "WorkEnd",
                table: "Job",
                newName: "JobEndEntryDate");

            migrationBuilder.RenameColumn(
                name: "QueueDate",
                table: "Job",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "JobEndDate",
                table: "Job",
                newName: "AppStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppEndDate",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AppEntryDate",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppEndDate",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "AppEntryDate",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Job",
                newName: "WorkStart");

            migrationBuilder.RenameColumn(
                name: "JobEndEntryDate",
                table: "Job",
                newName: "WorkEnd");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Job",
                newName: "QueueDate");

            migrationBuilder.RenameColumn(
                name: "AppStartDate",
                table: "Job",
                newName: "JobEndDate");
        }
    }
}
