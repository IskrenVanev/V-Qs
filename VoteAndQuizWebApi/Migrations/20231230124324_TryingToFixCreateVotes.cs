using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoteAndQuizWebApi.Migrations
{
    /// <inheritdoc />
    public partial class TryingToFixCreateVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_CreatorId",
                table: "Votes");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Votes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_CreatorId",
                table: "Votes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_CreatorId",
                table: "Votes");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Votes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_CreatorId",
                table: "Votes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
