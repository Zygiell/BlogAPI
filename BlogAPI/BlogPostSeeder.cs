using BlogAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI
{
    public class BlogPostSeeder
    {
        private readonly BlogDbContext _dbContext;

        public BlogPostSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Posts.Any())
                {
                    var templatePost = GetTemplatePost();
                    _dbContext.Posts.Add(templatePost);
                    _dbContext.SaveChanges();
                    
                }
            }
        }

        private Post GetTemplatePost()
        {
            var templatePost = new Post()
            {
                PostTitle = "Example Title",
                PostBody = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ac fermentum sapien. " +
                "Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; " +
                "Sed mattis magna ut hendrerit vulputate. In sollicitudin ante et augue vestibulum viverra vitae vel diam",
                CanComment = true,
                Comments = new List<Comment>()
                {
                    new Comment()
                    {
                        CommentBody = "Ut consectetur convallis leo eget facilisis. Sed faucibus sit amet turpis ut volutpat. Curabitur volutpat eu erat eu volutpat. " +
                        "Vestibulum vitae eros consequat."
                    }
                }


            };
            return templatePost;
        }
    }
}
