using BlogAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI
{
    public class BlogSeeder
    {
        private readonly BlogDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public BlogSeeder(BlogDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Posts.Any())
                {
                    var templatePost = GetTemplatePost();
                    _dbContext.Posts.Add(templatePost);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any(u => u.RoleId == 3))
                {
                    var adminAccount = GetAdminAccount();
                    _dbContext.Users.Add(adminAccount);
                    _dbContext.SaveChanges();
                };
            }
        }

        private User GetAdminAccount()
        {
            var admin = new User()
            {
                Email = "admin@admin.com",
                RoleId = 3,
                FirstName = "",
                LastName = "",
                City = ""
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, "admin");

            return admin;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Editor"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
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
                        CommentBody = "Ut consectetur convallis leo eget facilisis. Sed faucibus sit amet turpis ut volutpat. " +
                        "Curabitur volutpat eu erat eu volutpat. " +
                        "Vestibulum vitae eros consequat."
                    }
                }
            };
            return templatePost;
        }
    }
}