using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoteAndQuizWebApi.Migrations
{
    /// <inheritdoc />
    public partial class WinnerQuizOptionNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizOptions_Quizzes_QuizId",
                table: "QuizOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizOptions",
                table: "QuizOptions");

            migrationBuilder.RenameTable(
                name: "QuizOptions",
                newName: "WinnerQuizOptions");

            migrationBuilder.RenameIndex(
                name: "IX_QuizOptions_QuizId",
                table: "WinnerQuizOptions",
                newName: "IX_WinnerQuizOptions_QuizId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WinnerQuizOptions",
                table: "WinnerQuizOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WinnerQuizOptions_Quizzes_QuizId",
                table: "WinnerQuizOptions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinnerQuizOptions_Quizzes_QuizId",
                table: "WinnerQuizOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WinnerQuizOptions",
                table: "WinnerQuizOptions");

            migrationBuilder.RenameTable(
                name: "WinnerQuizOptions",
                newName: "QuizOptions");

            migrationBuilder.RenameIndex(
                name: "IX_WinnerQuizOptions_QuizId",
                table: "QuizOptions",
                newName: "IX_QuizOptions_QuizId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizOptions",
                table: "QuizOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizOptions_Quizzes_QuizId",
                table: "QuizOptions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
