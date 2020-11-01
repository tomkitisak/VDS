using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updjobagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Designation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Designation",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
