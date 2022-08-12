using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface ICommentService
    {
        void CommentUpVote(int postId, int commentId);
        void CommentDownVote(int postId, int commentId);
        int CreateNewComment(int postId, CreateNewCommentDto dto);
        void UpdateComment(int postId, int commentId, UpdateCommentDto dto);
        void RemoveCommentById(int postId, int commentId);
        List<CommentDto> GetAllComments(int postId);
        CommentDto GetCommentById(int postId, int commentId);

    }
}