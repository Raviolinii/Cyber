using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyber.Data.Migrations
{
    public partial class addedroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "AspNetRoles",
                 columns: new[] { "Id", "Name", "NormalizedName" },
                 values: new object[] { "1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "Name", "NormalizedName" },
            values: new object[] { "2", "User", "User" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
