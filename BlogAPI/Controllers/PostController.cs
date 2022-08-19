using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/blog")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // UPVOTE DOWNVOTE REGION

        #region Post ratings (upvote, downvote)

        //Add +1 score to Post Rating
        [HttpPost("post/{id}/upvote")]
        public async Task<IActionResult> PostUpvoteAsync([FromRoute] int id)
        {
            await _postService.PostUpVoteAsync(id);
            return Ok();
        }

        //Subtract -1 score from Post Rating
        [HttpPost("post/{id}/downvote")]
        public async Task<IActionResult> PostDownvoteAsync([FromRoute] int id)
        {
            await _postService.PostDownVoteAsync(id);
            return Ok();
        }

        #endregion Post ratings (upvote, downvote)

        // ADD UPDATE REMOVE POST REGION

        #region Add new Post // Update Post // Remove Post

        //ADD NEW POST
        [HttpPost("post/add")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> AddPostAsync([FromBody] CreateNewPostDto dto)
        {
            var postId = await _postService.CreateNewPostAsync(dto);

            return Created($"/api/blog/post/{postId}", null);
        }

        // EDIT POST by ID
        [HttpPut("post/edit/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] int id, [FromBody] UpdatePostDto dto)
        {
            await _postService.UpdatePostAsync(id, dto);

            return Ok();
        }

        // REMOVE POST BY ID
        [HttpDelete("post/remove/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> RemovePostAsync([FromRoute] int id)
        {
            await _postService.RemovePostAsync(id);

            return NoContent();
        }

        #endregion Add new Post // Update Post // Remove Post

        // GET POST GET ALL POST REGION

        #region Get post by id // Get All Posts

        // GET ALL POSTS
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] PostQuery query)
        {
            var posts = await _postService.GetAllPostsAsync(query);

            return Ok(posts);
        }

        // GET POST BY ID
        [HttpGet("post/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostByIdAsync([FromRoute] int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            return Ok(post);
        }

        #endregion Get post by id // Get All Posts
    }
}