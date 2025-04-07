using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddStarsCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("4decbdd8-070d-42be-b86d-3db085da1a19"));

            migrationBuilder.AddColumn<int>(
                name: "StarsAmount",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("b6571210-b6ac-41a3-85dc-6f298d66365c"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("b6571210-b6ac-41a3-85dc-6f298d66365c"));

            migrationBuilder.DropColumn(
                name: "StarsAmount",
                table: "Student");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "Username" },
                values: new object[] { new Guid("4decbdd8-070d-42be-b86d-3db085da1a19"), "TestUser@test.com", null, 100.45, "TestPassword", null, "TestUserName" });
        }
    }
}
