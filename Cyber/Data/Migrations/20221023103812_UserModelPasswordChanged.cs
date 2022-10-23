using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyber.Data.Migrations
{
    public partial class UserModelPasswordChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PasswordChanged",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordChanged",
                table: "AspNetUsers");
        }
    }
}
