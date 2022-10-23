using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyber.Data.Migrations
{
    public partial class UserModelPasswordExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpirationDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "PasswordExpirationEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordExpirationDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordExpirationEnabled",
                table: "AspNetUsers");
        }
    }
}
