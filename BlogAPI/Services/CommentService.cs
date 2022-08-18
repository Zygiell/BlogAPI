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
using System.Text;
using System.Threading.Tasks;

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
        public void CommentUpVote(int postId, int commentId)
        {
            var post = FindPostById(postId);
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var user = _userContextService.GetUserId;
            var isCommentVotedByUser = _dbContext.CommentVotes.FirstOrDefault(u => u.UserId == user && u.CommentId == commentId);

            if (isCommentVotedByUser == null)
            {
                comment.CommentRating += 1;
                _dbContext.CommentVotes.Add(new CommentVote
                {
                    UserId = user.Value,
                    CommentId = commentId,
                    IsCommentUpVotedByUser = true
                });
                _dbContext.SaveChanges();
            }
            else if (!isCommentVotedByUser.IsCommentUpVotedByUser)
            {
                comment.CommentRating += 1;
                isCommentVotedByUser.IsCommentUpVotedByUser = true;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new BadRequestException("Comment already upvoted by user");
            }


        }


        public void CommentDownVote(int postId, int commentId)
        {
            var post = FindPostById(postId);
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var user = _userContextService.GetUserId;
            var isCommentVotedByUser = _dbContext.CommentVotes.FirstOrDefault(u => u.UserId == user && u.CommentId == commentId);

            if (isCommentVotedByUser == null)
            {
                comment.CommentRating -= 1;
                _dbContext.CommentVotes.Add(new CommentVote
                {
                    UserId = user.Value,
                    CommentId = commentId,
                    IsCommentUpVotedByUser = false
                });
                _dbContext.SaveChanges();
            }
            else if (isCommentVotedByUser.IsCommentUpVotedByUser)
            {
                comment.CommentRating -= 1;
                isCommentVotedByUser.IsCommentUpVotedByUser = false;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new BadRequestException("Comment already downvoted by user");
            }
        }
        #endregion


        // Get REGION
        #region Get all comments // Get comment by id
        public List<CommentDto> GetAllComments(int postId)
        {
            var post = FindPostById(postId);

            var commentDtos = _mapper.Map<List<CommentDto>>(post.Comments);

            return commentDtos;
        }


        public CommentDto GetCommentById(int postId, int commentId)
        {
            var post = FindPostById(postId);
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null || comment.PostId != postId)
            {
                throw new NotFoundException("Comment not found");
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }
        #endregion


        // Create/Update/Delete REGION
        #region Create/Update/Delete comment
        public int CreateNewComment(int postId, CreateNewCommentDto dto)
        {
            var post = FindPostById(postId);

            if (post.CanComment)
            {
                var commentEntity = _mapper.Map<Comment>(dto);
                commentEntity.PostId = postId;
                commentEntity.CreatedByUserId = _userContextService.GetUserId;

                _dbContext.Comments.Add(commentEntity);
                _dbContext.SaveChanges();

                return commentEntity.Id;
            }

            else
            {
                throw new BadRequestException("Post cannot be commented");
            }



        }

        public void UpdateComment(int postId, int commentId, UpdateCommentDto dto)
        {
            var post = FindPostById(postId);
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

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
            _dbContext.SaveChanges();
        }

        public void RemoveCommentById(int postId, int commentId)
        {
            var post = FindPostById(postId);
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);

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
            _dbContext.SaveChanges();

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
