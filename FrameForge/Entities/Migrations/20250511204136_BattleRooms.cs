using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class BattleRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("a20a1008-a76a-4a19-8edc-ff7c5ba60650"));

            migrationBuilder.CreateTable(
                name: "BattleRooms",
                columns: table => new
                {
                    roomId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Player1Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Player2Id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Questions = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleRooms", x => x.roomId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("0d744798-58ed-4995-9124-f4ba3da2c165"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleRooms");

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("0d744798-58ed-4995-9124-f4ba3da2c165"));

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("a20a1008-a76a-4a19-8edc-ff7c5ba60650"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
