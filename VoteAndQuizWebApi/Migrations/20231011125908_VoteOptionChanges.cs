using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoteAndQuizWebApi.Migrations
{
    /// <inheritdoc />
    public partial class VoteOptionChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteOptions_Votes_VoteId",
                table: "VoteOptions");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "VoteOptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteOptions_Votes_VoteId",
                table: "VoteOptions",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteOptions_Votes_VoteId",
                table: "VoteOptions");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "VoteOptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteOptions_Votes_VoteId",
                table: "VoteOptions",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }
    }
}
