using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMapper _mapper;


        public PostService(BlogDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        #region Post UpVote // Post DownVote

        public void PostDownVote(int id)
        {
            var post = _dbContext
                .Posts
                .FirstOrDefault(p => p.Id == id);

            post.PostRating -= 1;

            _dbContext.SaveChanges();
        }


        public void PostUpVote(int id)
        {
            var post = _dbContext
                .Posts
                .FirstOrDefault(p => p.Id == id);

            post.PostRating += 1;

            _dbContext.SaveChanges();
        }

        #endregion


        #region Add/Remove/Update Post
        public int CreateNewPost(CreateNewPostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return post.Id;

        }
        public void RemovePost(int id)
        {
            var post = _dbContext
                .Posts
                .FirstOrDefault(p => p.Id == id);


            _dbContext.Posts.Remove(post);

            _dbContext.SaveChanges();

        }

        public void UpdatePost(int id, UpdatePostDto dto)
        {
            var post = _dbContext
                .Posts
                .FirstOrDefault(p => p.Id == id);


                post.PostTitle = dto.PostTitle;
                post.PostBody = dto.PostBody;
                post.CanComment = dto.CanComment;


            _dbContext.SaveChanges();

        }

        #endregion


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
            var post = _dbContext
                .Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return null;

            var result = _mapper.Map<PostDto>(post);
            result.PostCommentsCount = result.Comments.Count();

            return result;

        }
        #endregion
    }
}
