using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI.Migrations
{
    public partial class VotingTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPostUpVotedByUser",
                table: "PostVotes",
                newName: "IsUpvoted");

            migrationBuilder.RenameColumn(
                name: "IsCommentUpVotedByUser",
                table: "CommentVotes",
                newName: "IsUpvoted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUpvoted",
                table: "PostVotes",
                newName: "IsPostUpVotedByUser");

            migrationBuilder.RenameColumn(
                name: "IsUpvoted",
                table: "CommentVotes",
                newName: "IsCommentUpVotedByUser");
        }
    }
}
