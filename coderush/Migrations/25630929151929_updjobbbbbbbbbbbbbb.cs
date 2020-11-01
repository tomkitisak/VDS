using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updjobbbbbbbbbbbbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Department_DepartmentId",
                table: "Coordinator");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Designation_DesignationId",
                table: "Coordinator");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator");

            migrationBuilder.RenameColumn(
                name: "WorkDate",
                table: "Job",
                newName: "WorkStart");

            migrationBuilder.RenameColumn(
                name: "ListDate",
                table: "Job",
                newName: "WorkEnd");

            migrationBuilder.AddColumn<DateTime>(
                name: "JobEndDate",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "QueueDate",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospitalId",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignationId",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Department_DepartmentId",
                table: "Coordinator",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Designation_DesignationId",
                table: "Coordinator",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Department_DepartmentId",
                table: "Coordinator");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Designation_DesignationId",
                table: "Coordinator");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator");

            migrationBuilder.DropColumn(
                name: "JobEndDate",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "QueueDate",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "WorkStart",
                table: "Job",
                newName: "WorkDate");

            migrationBuilder.RenameColumn(
                name: "WorkEnd",
                table: "Job",
                newName: "ListDate");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "HospitalId",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DesignationId",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Department_DepartmentId",
                table: "Coordinator",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Designation_DesignationId",
                table: "Coordinator",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
