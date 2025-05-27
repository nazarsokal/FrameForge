using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class DeletedTimeCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("a4277fd7-66aa-47a6-8e91-468dfaf64a80"));

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "BattleRooms");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("c4889752-3472-4be1-987a-e08f446c5da5"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("c4889752-3472-4be1-987a-e08f446c5da5"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "BattleRooms",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "BattleRooms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("a4277fd7-66aa-47a6-8e91-468dfaf64a80"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
