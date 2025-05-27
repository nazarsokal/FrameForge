using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseSubmissionModelChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "ExerciseSubmissions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "MoneyReward",
                table: "ExerciseSubmissions",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StarsReward",
                table: "ExerciseSubmissions",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "ExerciseSubmissions");

            migrationBuilder.DropColumn(
                name: "MoneyReward",
                table: "ExerciseSubmissions");

            migrationBuilder.DropColumn(
                name: "StarsReward",
                table: "ExerciseSubmissions");
        }
    }
}
