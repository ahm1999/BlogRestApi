

using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.utils
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public DbSet<Blog> Blogs { get; set; }
        
    }
}