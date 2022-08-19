using AutoMapper;
using BlogAPI.Authorization;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public CommentService(BlogDbContext dbContext, IMapper mapper,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        // Upvote Downvote REGION

        #region Upvote/Downvote comment

        public async Task CommentUpVoteAsync(int postId, int commentId)
        {
            var post = await FindPostById(postId);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var user = _userContextService.GetUserId;
            var isCommentVotedByUser = await _dbContext.CommentVotes.FirstOrDefaultAsync(u => u.UserId == user && u.CommentId == commentId);

            if (isCommentVotedByUser == null)
            {
                comment.CommentRating += 1;
                await _dbContext.CommentVotes.AddAsync(new CommentVote
                {
                    UserId = user.Value,
                    CommentId = commentId,
                    IsCommentUpVotedByUser = true
                });
                await _dbContext.SaveChangesAsync();
            }
            else if (!isCommentVotedByUser.IsCommentUpVotedByUser)
            {
                comment.CommentRating += 1;
                isCommentVotedByUser.IsCommentUpVotedByUser = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException("Comment already upvoted by user");
            }
        }

        public async Task CommentDownVoteAsync(int postId, int commentId)
        {
            var post = await FindPostById(postId);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var user = _userContextService.GetUserId;
            var isCommentVotedByUser = await _dbContext.CommentVotes.FirstOrDefaultAsync(u => u.UserId == user && u.CommentId == commentId);

            if (isCommentVotedByUser == null)
            {
                comment.CommentRating -= 1;
                await _dbContext.CommentVotes.AddAsync(new CommentVote
                {
                    UserId = user.Value,
                    CommentId = commentId,
                    IsCommentUpVotedByUser = false
                });
                await _dbContext.SaveChangesAsync();
            }
            else if (isCommentVotedByUser.IsCommentUpVotedByUser)
            {
                comment.CommentRating -= 1;
                isCommentVotedByUser.IsCommentUpVotedByUser = false;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException("Comment already downvoted by user");
            }
        }

        #endregion Upvote/Downvote comment

        // Get REGION

        #region Get all comments // Get comment by id

        public async Task<List<CommentDto>> GetAllCommentsAsync(int postId)
        {
            var post = await FindPostById(postId);

            var commentDtos = _mapper.Map<List<CommentDto>>(post.Comments);

            return commentDtos;
        }

        public async Task<CommentDto> GetCommentByIdAsync(int postId, int commentId)
        {
            var post = await FindPostById(postId);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        #endregion Get all comments // Get comment by id

        // Create/Update/Delete REGION

        #region Create/Update/Delete comment

        public async Task<int> CreateNewCommentAsync(int postId, CreateNewCommentDto dto)
        {
            var post = await FindPostById(postId);

            if (post.CanComment)
            {
                var commentEntity = _mapper.Map<Comment>(dto);
                commentEntity.PostId = postId;
                commentEntity.CreatedByUserId = _userContextService.GetUserId;

                await _dbContext.Comments.AddAsync(commentEntity);
                await _dbContext.SaveChangesAsync();

                return commentEntity.Id;
            }
            else
            {
                throw new BadRequestException("Post cannot be commented");
            }
        }

        public async Task UpdateCommentAsync(int postId, int commentId, UpdateCommentDto dto)
        {
            var post = await FindPostById(postId);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, comment,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            comment.CommentBody = dto.CommentBody;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveCommentByIdAsync(int postId, int commentId)
        {
            var post = await FindPostById(postId);
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, comment,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }

        #endregion Create/Update/Delete comment

        /// <summary>
        /// Finds specific post using id in database, and returns it.
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>Post matching id</returns>
        /// <exception cref="NotFoundException">Post with desired id do not exist</exception>
        private async Task<Post> FindPostById(int id)
        {
            var post = await _dbContext
                .Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) throw new NotFoundException($"Post with id: {id} not found");

            return post;
        }
    }
}