using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
