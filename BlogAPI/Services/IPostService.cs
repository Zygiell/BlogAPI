using BlogAPI.Models;
using System.Security.Claims;

namespace BlogAPI.Services
{
    public interface IPostService
    {
        void PostUpVote(int id);
        void PostDownVote(int id);
        int CreateNewPost(CreateNewPostDto dto);
        void UpdatePost(int id, UpdatePostDto dto);
        void RemovePost(int id);
        IEnumerable<PostDto> GetAllPosts();
        PostDto GetPostById(int id);
    }
}