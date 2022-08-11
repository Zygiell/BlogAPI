using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IPostService
    {
        int CreateNewPost(CreateNewPostDto dto);
        IEnumerable<PostDto> GetAllPosts();
        PostDto GetPostById(int id);
        void PostDownVote(int id);
        void PostUpVote(int id);
        void RemovePost(int id);
        void UpdatePost(int id, UpdatePostDto dto);
    }
}