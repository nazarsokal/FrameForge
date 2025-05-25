using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TimeForHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("2170211e-dbd7-4653-a614-36736198cd10"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "BattleHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("866e1126-0c39-41b9-9c04-9309b532ce1b"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("866e1126-0c39-41b9-9c04-9309b532ce1b"));

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "BattleHistory");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("2170211e-dbd7-4653-a614-36736198cd10"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
