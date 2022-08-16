using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ActionResult CommentUpvote([FromRoute] int postId, [FromRoute] int commentId)
        {
            _commentService.CommentUpVote(postId, commentId);

            return Ok();
        }


        //Subtract -1 score from Comment Rating
        [HttpPost("{commentId}/downvote")]
        public ActionResult CommentDownvote([FromRoute] int postId, [FromRoute] int commentId)
        {
            _commentService.CommentDownVote(postId, commentId);

            return Ok();
        }
        #endregion


        // ADD UPDATE REMOVE COMMENT REGION
        #region ADD/UPDATE/REMOVE


        //ADD NEW COMMENT
        [HttpPost("new")]
        public ActionResult AddComment([FromRoute] int postId, CreateNewCommentDto dto)
        {
            var newCommentId = _commentService.CreateNewComment(postId, dto);

            return Created($"api/post/{postId}/comment/{newCommentId}", null);
        }


        //EDIT COMMENT BY ID
        [HttpPut("{commentId}/edit")]
        public ActionResult UpdateComment([FromRoute] int postId, [FromRoute] int commentId, [FromBody] UpdateCommentDto dto)
        {
            _commentService.UpdateComment(postId, commentId, dto);
            return Ok();
        }


        //DELETE COMMENT BY ID
        [HttpDelete("{commentId}")]
        public ActionResult RemoveCommentById([FromRoute]int postId, [FromRoute]int commentId)
        {
            _commentService.RemoveCommentById(postId, commentId);

            return NoContent();
        }



        #endregion


        // GET COMMENT GET ALL COMMENTS REGION
        #region GET


        //GET ALL COMMENTS
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CommentDto>> GetAllComments([FromRoute]int postId)
        {
            var comments = _commentService.GetAllComments(postId);

            return Ok(comments);
        }


        // GET COMMENT BY ID
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public ActionResult<CommentDto> GetCommentById([FromRoute]int postId, [FromRoute]int commentId)
        {
            var comment = _commentService.GetCommentById(postId, commentId);

            return Ok(comment);
        }
        #endregion
    }
}
