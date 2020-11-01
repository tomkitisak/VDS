using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class updjobbbbbbbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Job",
                newName: "Remark2");

            migrationBuilder.AddColumn<string>(
                name: "Remark1",
                table: "Job",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "GroupCoordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employee",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctor",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Director",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Coordinator",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark1",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "Remark2",
                table: "Job",
                newName: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "GroupCoordinator",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Director",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Coordinator",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
