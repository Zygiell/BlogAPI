using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IPostService
    {
        void PostUpVote(int id);

        void PostDownVote(int id);

        int CreateNewPost(CreateNewPostDto dto);

        void UpdatePost(int id, UpdatePostDto dto);

        void RemovePost(int id);

        PagedResult<PostDto> GetAllPosts(PostQuery query);

        PostDto GetPostById(int id);
    }
}