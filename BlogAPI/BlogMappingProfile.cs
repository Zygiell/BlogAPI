using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Models;

namespace BlogAPI
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Post, PostDto>();

            CreateMap<Comment, CommentDto>();

            CreateMap<CreateNewPostDto, Post>();

            CreateMap<CreateNewCommentDto, Comment>();

            CreateMap<User, UserDto>();
        }
    }
}