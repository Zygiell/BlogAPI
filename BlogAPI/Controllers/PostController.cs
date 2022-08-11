using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        #region Add new Post // Delete Post // Update Post
        [HttpPut("post/edit/{id}")]
        public ActionResult UpdatePost([FromRoute]int id, [FromBody]UpdatePostDto dto)
        {
            _postService.UpdatePost(id, dto);

            return Ok();
        }

        [HttpPost("post/add")]
        public ActionResult AddPost([FromBody]CreateNewPostDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postId = _postService.CreateNewPost(dto);


            return Created($"/api/blog/post/{postId}", null);
        }

        [HttpDelete("post/remove/{id}")]
        public ActionResult RemovePost([FromRoute] int id)
        {
            _postService.RemovePost(id);

            return NoContent();
        }

        #endregion

        #region Post ratings (upvote, downvote)

        //Subtract -1 score from Post Rating
        [HttpPost("post/{id}/downvote")]
        public ActionResult<PostDto> PostDownvote([FromRoute] int id)
        {
            var doesPostExist = _postService.GetPostById(id);

            if (doesPostExist == null)
            {
                return NotFound();
            }
            _postService.PostDownVote(id);
            return Ok();
        }



        //Add +1 score to Post Rating
        [HttpPost("post/{id}/upvote")]
        public ActionResult<PostDto> PostUpvote([FromRoute] int id)
        {
            var doesPostExist = _postService.GetPostById(id);

            if (doesPostExist == null)
            {
                return NotFound();
            }
            _postService.PostUpVote(id);
            return Ok();
        }
        #endregion

        #region Get post by id // Get All Posts
        // GET POST BY ID
        [HttpGet("post/{id}")]
        public ActionResult<PostDto> GetPostById([FromRoute] int id)
        {
            var post = _postService.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }


        // GET ALL POSTS
        [HttpGet]
        public ActionResult<IEnumerable<PostDto>> GetAll()
        {
            var posts = _postService.GetAllPosts();
            

            return Ok(posts);
        }

        #endregion
    }
}
