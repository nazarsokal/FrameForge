using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Creating_MentorsQuotes_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("afbfb446-a78a-406d-8ce1-3d2522e8bd1e"));

            migrationBuilder.CreateTable(
                name: "MentorNpcQuotes",
                columns: table => new
                {
                    QuoteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LevelNumber = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    Quote = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorNpcQuotes", x => x.QuoteId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("f57016ea-df17-4050-b9b0-b9bc3314e3c7"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorNpcQuotes");

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: new Guid("f57016ea-df17-4050-b9b0-b9bc3314e3c7"));

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentId", "Email", "GoogleId", "MoneyAmount", "Password", "Picture", "StarsAmount", "Username" },
                values: new object[] { new Guid("afbfb446-a78a-406d-8ce1-3d2522e8bd1e"), "TestUser@test.com", null, 100.45, "TestPassword", null, 0, "TestUserName" });
        }
    }
}
