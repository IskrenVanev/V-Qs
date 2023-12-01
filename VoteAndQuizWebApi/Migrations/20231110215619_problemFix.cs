using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoteAndQuizWebApi.Migrations
{
    /// <inheritdoc />
    public partial class problemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAnswers_AspNetUsers_UserId",
                table: "UserQuizAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAnswers_Quizzes_QuizId",
                table: "UserQuizAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserQuizAnswers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAnswers_AspNetUsers_UserId",
                table: "UserQuizAnswers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAnswers_Quizzes_QuizId",
                table: "UserQuizAnswers",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAnswers_AspNetUsers_UserId",
                table: "UserQuizAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAnswers_Quizzes_QuizId",
                table: "UserQuizAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserQuizAnswers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAnswers_AspNetUsers_UserId",
                table: "UserQuizAnswers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAnswers_Quizzes_QuizId",
                table: "UserQuizAnswers",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
