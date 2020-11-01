using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updDoctorType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTypeName",
                table: "DoctorType");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DoctorType",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "DoctorType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DoctorType");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "DoctorType");

            migrationBuilder.AddColumn<string>(
                name: "SubTypeName",
                table: "DoctorType",
                nullable: true);
        }
    }
}
