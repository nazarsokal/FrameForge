using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class LevelsEnrolledTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("b6571210-b6ac-41a3-85dc-6f298d66365c"));

            migrationBuilder.CreateTable(
                name: "LevelsEnrolled",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LevelTopicName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoneyReward = table.Column<double>(type: "double", nullable: false),
                    StarsReward = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelsEnrolled", x => x.StudentId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("c32655a7-b70a-4c0f-9a75-f5de92099776"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelsEnrolled");

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("c32655a7-b70a-4c0f-9a75-f5de92099776"));

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("b6571210-b6ac-41a3-85dc-6f298d66365c"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
