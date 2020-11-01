using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class upddepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Department",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Department",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Department");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Department",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
