using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class jobupdateeeeeeeeeeeeeeee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAppointed",
                table: "Job",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Job",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Job",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAppointed",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Job");
        }
    }
}
