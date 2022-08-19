using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    public partial class CommentVoteAddUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPostVoted",
                table: "CommentVotes",
                newName: "IsCommentVoted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCommentVoted",
                table: "CommentVotes",
                newName: "IsPostVoted");
        }
    }
}