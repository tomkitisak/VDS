using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class upd17112563 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Long",
                table: "Hospital",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Lat",
                table: "Hospital",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Long",
                table: "Hospital",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Lat",
                table: "Hospital",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
