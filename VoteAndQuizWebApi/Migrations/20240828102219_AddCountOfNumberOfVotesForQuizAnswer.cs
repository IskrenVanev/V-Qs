using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoteAndQuizWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCountOfNumberOfVotesForQuizAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "quizAnswerVotes",
                table: "UserQuizAnswers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quizAnswerVotes",
                table: "UserQuizAnswers");
        }
    }
}
