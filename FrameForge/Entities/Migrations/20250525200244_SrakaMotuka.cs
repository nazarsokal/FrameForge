using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SrakaMotuka : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSubmissions_Users_StudentSubmittedUserId",
                table: "ExerciseSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSubmissions_StudentSubmittedUserId",
                table: "ExerciseSubmissions");

            migrationBuilder.RenameColumn(
                name: "StudentSubmittedUserId",
                table: "ExerciseSubmissions",
                newName: "StudentSubmittedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentSubmittedId",
                table: "ExerciseSubmissions",
                newName: "StudentSubmittedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSubmissions_StudentSubmittedUserId",
                table: "ExerciseSubmissions",
                column: "StudentSubmittedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSubmissions_Users_StudentSubmittedUserId",
                table: "ExerciseSubmissions",
                column: "StudentSubmittedUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
