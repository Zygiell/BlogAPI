using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
        public ActionResult<PostDto> PostUpvote([FromRoute] int id)
        {
            _postService.PostUpVote(id);
            return Ok();
        }


        //Subtract -1 score from Post Rating
        [HttpPost("post/{id}/downvote")]
        public ActionResult<PostDto> PostDownvote([FromRoute] int id)
        {
            _postService.PostDownVote(id);
            return Ok();
        }


        #endregion


        // ADD UPDATE REMOVE POST REGION
        #region Add new Post // Update Post // Remove Post


        //ADD NEW POST
        [HttpPost("post/add")]
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult AddPost([FromBody] CreateNewPostDto dto)
        {
            var postId = _postService.CreateNewPost(dto);

            return Created($"/api/blog/post/{postId}", null);
        }


        // EDIT POST by ID
        [HttpPut("post/edit/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult UpdatePost([FromRoute]int id, [FromBody]UpdatePostDto dto)
        {
            _postService.UpdatePost(id, dto);

            return Ok();
        }


        // REMOVE POST BY ID
        [HttpDelete("post/remove/{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult RemovePost([FromRoute] int id)
        {
            _postService.RemovePost(id);

            return NoContent();
        }


        #endregion


        // GET POST GET ALL POST REGION
        #region Get post by id // Get All Posts

        // GET ALL POSTS
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<PostDto>> GetAll()
        {
            var posts = _postService.GetAllPosts();
            

            return Ok(posts);
        }

        // GET POST BY ID
        [HttpGet("post/{id}")]
        [AllowAnonymous]
        public ActionResult<PostDto> GetPostById([FromRoute] int id)
        {
            var post = _postService.GetPostById(id);

            return Ok(post);
        }

        #endregion
    }
}
