using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updjobtotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDoctors",
                table: "Job",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPatients",
                table: "Job",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDoctors",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "TotalPatients",
                table: "Job");
        }
    }
}
