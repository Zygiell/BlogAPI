using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/blog/post/{postId}/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //UPVOTE DOWNVOTE COMMENT REGION

        #region Upvote/Downvote comment

        //Add +1 score to Comment Rating
        [HttpPost("{commentId}/upvote")]
        public async Task<IActionResult> CommentUpvoteAsync([FromRoute] int postId, [FromRoute] int commentId)
        {
            await _commentService.CommentUpVoteAsync(postId, commentId);

            return Ok();
        }

        //Subtract -1 score from Comment Rating
        [HttpPost("{commentId}/downvote")]
        public async Task<IActionResult> CommentDownvoteAsync([FromRoute] int postId, [FromRoute] int commentId)
        {
            await _commentService.CommentDownVoteAsync(postId, commentId);

            return Ok();
        }

        #endregion Upvote/Downvote comment

        // ADD UPDATE REMOVE COMMENT REGION

        #region ADD/UPDATE/REMOVE

        //ADD NEW COMMENT
        [HttpPost("new")]
        public async Task<IActionResult> AddCommentAsync([FromRoute] int postId, CreateNewCommentDto dto)
        {
            var newCommentId = await _commentService.CreateNewCommentAsync(postId, dto);

            return Created($"api/post/{postId}/comment/{newCommentId}", null);
        }

        //EDIT COMMENT BY ID
        [HttpPut("{commentId}/edit")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int postId, [FromRoute] int commentId, [FromBody] UpdateCommentDto dto)
        {
            await _commentService.UpdateCommentAsync(postId, commentId, dto);
            return Ok();
        }

        //DELETE COMMENT BY ID
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> RemoveCommentByIdAsync([FromRoute] int postId, [FromRoute] int commentId)
        {
            await _commentService.RemoveCommentByIdAsync(postId, commentId);

            return NoContent();
        }

        #endregion ADD/UPDATE/REMOVE

        // GET COMMENT GET ALL COMMENTS REGION

        #region GET

        //GET ALL COMMENTS
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCommentsAsync([FromRoute] int postId)
        {
            var comments = await _commentService.GetAllCommentsAsync(postId);

            return Ok(comments);
        }

        // GET COMMENT BY ID
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentByIdAsync([FromRoute] int postId, [FromRoute] int commentId)
        {
            var comment = await _commentService.GetCommentByIdAsync(postId, commentId);

            return Ok(comment);
        }

        #endregion GET
    }
}