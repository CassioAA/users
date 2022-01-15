using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manager.Infra.Migrations
{
    public partial class LengthsChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "User",
                type: "VARCHAR(180)",
                maxLength: 180,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "User",
                type: "VARCHAR(140)",
                maxLength: 140,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "VARCHAR(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(180)",
                oldMaxLength: 180);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "User",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(180)",
                oldMaxLength: 180);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "User",
                type: "VARCHAR(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(140)",
                oldMaxLength: 140);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "VARCHAR(180)",
                maxLength: 180,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(80)",
                oldMaxLength: 80);
        }
    }
}
