using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class ddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator");

            migrationBuilder.AddColumn<int>(
                name: "UserLevel",
                table: "UserType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Patient",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HospitalId",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator");

            migrationBuilder.DropColumn(
                name: "UserLevel",
                table: "UserType");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Patient",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "HospitalId",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinator_Hospital_HospitalId",
                table: "Coordinator",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
