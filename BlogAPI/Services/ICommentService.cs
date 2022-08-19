using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface ICommentService
    {
        Task CommentUpVoteAsync(int postId, int commentId);

        Task CommentDownVoteAsync(int postId, int commentId);

        Task<int> CreateNewCommentAsync(int postId, CreateNewCommentDto dto);

        Task UpdateCommentAsync(int postId, int commentId, UpdateCommentDto dto);

        Task RemoveCommentByIdAsync(int postId, int commentId);

        Task<List<CommentDto>> GetAllCommentsAsync(int postId);

        Task<CommentDto> GetCommentByIdAsync(int postId, int commentId);
    }
}