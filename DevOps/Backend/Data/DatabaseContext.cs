using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {

        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectsStatus> ProjectsStatus { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<AssignedTasks> AssignedTasks { get; set; }
        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AssignedTasks>().HasKey(x => new { x.UserId, x.TaskId }); // Composite primary key

            builder.Entity<ProjectsStatus>().HasKey(x => new { x.UserId, x.ProjectId }); // Composite primary key
        }
    }
}