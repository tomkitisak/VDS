using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class upddiseasytype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PrefixType",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DiseaseType",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "DiseaseType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "DiseaseType");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PrefixType",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DiseaseType",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
