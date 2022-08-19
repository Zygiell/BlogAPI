using AutoMapper;
using BlogAPI.Authorization;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            , IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        // UPVOTE  DOWNVOTE REGION

        #region Post UpVote // Post DownVote

        public Task PostUpVoteAsync(int id)
        {
            var post = FindPostByIdAsync(id);
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

        public async Task PostDownVoteAsync(int id)
        {
            var post = await FindPostByIdAsync(id);
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
                await _dbContext.SaveChangesAsync();
            }
            else if (isPostVotedByUser.IsPostUpVotedByUser)
            {
                post.PostRating -= 1;
                isPostVotedByUser.IsPostUpVotedByUser = false;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException("Post already downvoted by user");
            }
        }

        #endregion Post UpVote // Post DownVote

        // ADD UDATE REMOVE REGION

        #region Add/Update/Remove Post

        public async Task<int> CreateNewPostAsync(CreateNewPostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
            post.CreatedByUserId = _userContextService.GetUserId;
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            return post.Id;
        }

        public async Task UpdatePostAsync(int id, UpdatePostDto dto)
        {
            var post = await FindPostByIdAsync(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, post,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            post.PostTitle = dto.PostTitle;
            post.PostBody = dto.PostBody;
            post.CanComment = dto.CanComment;

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePostAsync(int id)
        {
            _logger.LogError($"Post with id: {id} DELETE action invoked");

            var post = await FindPostByIdAsync(id);

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, post,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

             _dbContext.Posts.Remove(post);

            await _dbContext.SaveChangesAsync();
        }

        #endregion Add/Update/Remove Post

        // GET ALL GET BY ID REGION

        #region Get Post by ID // Get All Posts        
        public async Task<PagedResult<PostDto>> GetAllPostsAsync(PostQuery query)
        {
            var posts = _dbContext
                .Posts
                .Include(p => p.Comments)
                .Where(s => query.SearchPhrase == null || (s.PostBody.ToLower().Contains(query.SearchPhrase)
                                                      || s.PostTitle.ToLower().Contains(query.SearchPhrase)));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Post, object>>>
                {
                    {nameof(Post.PostRating), p => p.PostRating},
                    {nameof(Post.PostCreationDate), p => p.PostCreationDate}
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                posts = query.SortDirection == SortDirection.ASCENDING
                    ? posts.OrderBy(selectedColumn)
                    : posts.OrderByDescending(selectedColumn);
            }

            var totalItemsCount = posts.Count();

            var result = _mapper.Map<List<PostDto>>(posts);

            result.ForEach(p => p.PostCommentsCount = p.Comments.Count());

            if (query.PageNumber > 0 && query.PageSize > 0)
            {
                posts = posts.Skip(query.PageSize * (query.PageNumber - 1))
                     .Take(query.PageSize);

                var inIfPageResult = new PagedResult<PostDto>(result, totalItemsCount, query.PageSize, query.PageNumber);

                return await Task.FromResult(inIfPageResult);
            }

            var pageResult = new PagedResult<PostDto>(result, totalItemsCount, totalItemsCount, 1);

            return  await Task.FromResult(pageResult);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await FindPostByIdAsync(id);

            var result = _mapper.Map<PostDto>(post);
            result.PostCommentsCount = result.Comments.Count();

            return result;
        }

        #endregion Get Post by ID // Get All Posts

        /// <summary>
        /// Finds specific post using id in database, and returns it.
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>Post matching id</returns>
        /// <exception cref="NotFoundException">Post with desired id do not exist</exception>
        private async Task<Post> FindPostByIdAsync(int id)
        {
            var post = _dbContext
                .Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) throw new NotFoundException($"Post with id: {id} not found");

            return await post;
        }
    }
}