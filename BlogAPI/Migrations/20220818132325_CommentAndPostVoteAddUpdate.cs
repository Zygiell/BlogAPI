using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    public partial class CommentAndPostVoteAddUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPostVoted",
                table: "PostVotes",
                newName: "IsPostUpVotedByUser");

            migrationBuilder.RenameColumn(
                name: "IsCommentVoted",
                table: "CommentVotes",
                newName: "IsCommentUpVotedByUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPostUpVotedByUser",
                table: "PostVotes",
                newName: "IsPostVoted");

            migrationBuilder.RenameColumn(
                name: "IsCommentUpVotedByUser",
                table: "CommentVotes",
                newName: "IsCommentVoted");
        }
    }
}