using Crowdfund.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .ToTable("User");

            modelBuilder
                .Entity<Project>()
                .ToTable("Project");

            modelBuilder
                .Entity<RewardPackage>()
                .ToTable("RewardPackage");

            modelBuilder
                .Entity<Post>()
                .ToTable("Post");

            modelBuilder
                .Entity<Media>()
                .ToTable("Media");

            modelBuilder
                .Entity<UserProjectReward>()
                .ToTable("UserProjectReward");
        }
    }
}