using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IPostService
    {
        Task PostUpVoteAsync(int id);

        Task PostDownVoteAsync(int id);

        Task<int> CreateNewPostAsync(CreateNewPostDto dto);

        Task UpdatePostAsync(int id, UpdatePostDto dto);

        Task RemovePostAsync(int id);

        Task<PagedResult<PostDto>> GetAllPostsAsync(PostQuery query);

        Task<PostDto> GetPostByIdAsync(int id);
    }
}