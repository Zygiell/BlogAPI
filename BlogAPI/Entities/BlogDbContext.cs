using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Entities
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(p => p.PostTitle)
                .IsRequired()
                .HasMaxLength(300);

            modelBuilder.Entity<Comment>()
                .Property(p => p.CommentBody)
                .IsRequired()
                .HasMaxLength(2000);

            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<PostVote>()
                .HasKey(p => new {p.PostId, p.UserId});

            modelBuilder.Entity<CommentVote>()
                .HasKey(p => new { p.CommentId, p.UserId });


        }

    }
}
