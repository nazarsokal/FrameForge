using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBattlesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("0d744798-58ed-4995-9124-f4ba3da2c165"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "BattleRooms",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player1Score",
                table: "BattleRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2Score",
                table: "BattleRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "BattleRooms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BattleRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerId",
                table: "BattleRooms",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("a4277fd7-66aa-47a6-8e91-468dfaf64a80"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("a4277fd7-66aa-47a6-8e91-468dfaf64a80"));

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "Player1Score",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "Player2Score",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BattleRooms");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "BattleRooms");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("0d744798-58ed-4995-9124-f4ba3da2c165"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
