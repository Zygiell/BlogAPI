using AutoMapper;
using BlogAPI.Authorization;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public PostService(BlogDbContext dbContext, IMapper mapper, ILogger<PostService> logger
            ,IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }


        // UPVOTE  DOWNVOTE REGION
        #region Post UpVote // Post DownVote


        public void PostUpVote(int id)
        {
            var post = FindPostById(id);
            var user = _userContextService.GetUserId;
            var isPostVotedByUser = _dbContext.PostVotes.FirstOrDefault(u => u.UserId == user && u.PostId == id);
            


            if (isPostVotedByUser == null)
            {
                post.PostRating += 1;
                _dbContext.PostVotes.Add(new PostVote
                {
                    UserId = user.Value,
                    PostId = id,
                    IsPostUpVotedByUser = true
                });
                _dbContext.SaveChanges();
            }
            else if (!isPostVotedByUser.IsPostUpVotedByUser)
            {
                post.PostRating += 1;
                isPostVotedByUser.IsPostUpVotedByUser = true;
                _dbContext.SaveChanges();
            }
            
            
            else
            {
                throw new BadRequestException("Post already upvoted by user");
            }


        }


        public void PostDownVote(int id)
        {
            var post = FindPostById(id);
            var user = _userContextService.GetUserId;
            var isPostVotedByUser = _dbContext.PostVotes.FirstOrDefault(u => u.UserId == user && u.PostId == id);

            if (isPostVotedByUser == null)
            {
                post.PostRating -= 1;
                _dbContext.PostVotes.Add(new PostVote
                {
                    UserId = user.Value,
                    PostId = id,
                    IsPostUpVotedByUser = false
                });
                _dbContext.SaveChanges();
            }
            else if (isPostVotedByUser.IsPostUpVotedByUser)
            {
                post.PostRating -= 1;
                isPostVotedByUser.IsPostUpVotedByUser = false;
                _dbContext.SaveChanges();
            }


            else
            {
                throw new BadRequestException("Post already downvoted by user");
            }
        }


        #endregion

        
        // ADD UDATE REMOVE REGION
        #region Add/Update/Remove Post


        public int CreateNewPost(CreateNewPostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
            post.CreatedByUserId = _userContextService.GetUserId;
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return post.Id;

        }


        public void UpdatePost(int id, UpdatePostDto dto)
        {            
            var post = FindPostById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, post, 
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            post.PostTitle = dto.PostTitle;
            post.PostBody = dto.PostBody;
            post.CanComment = dto.CanComment;


            _dbContext.SaveChanges();

        }


        public void RemovePost(int id)
        {
            _logger.LogError($"Post with id: {id} DELETE action invoked");

            var post = FindPostById(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, post,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Posts.Remove(post);

            _dbContext.SaveChanges();

        }


        #endregion


        // GET ALL GET BY ID REGION
        #region Get Post by ID // Get All Posts


        public IEnumerable<PostDto> GetAllPosts()
        {
            var posts = _dbContext
                .Posts
                .Include(p => p.Comments)
                .ToList();

            var result = _mapper.Map<List<PostDto>>(posts);
            result.ForEach(p => p.PostCommentsCount = p.Comments.Count());

            return result;
        }


        public PostDto GetPostById(int id)
        {
            var post = FindPostById(id);


            var result = _mapper.Map<PostDto>(post);
            result.PostCommentsCount = result.Comments.Count();

            return result;

        }
        #endregion




        /// <summary>
        /// Finds specific post using id in database, and returns it.
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>Post matching id</returns>
        /// <exception cref="NotFoundException">Post with desired id do not exist</exception>
        private Post FindPostById(int id)
        {
            var post = _dbContext
                .Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == id);
            if (post == null) throw new NotFoundException($"Post with id: {id} not found");

            return post;
        }
    }
}
