using System;
using System.Collections.Generic;

namespace Crowdfund.Core.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public DateTime DueTo { get; set; }
        
        public string MainImageUrl { get; set; }
        
        public int Goal { get; set; }
        
        public IList<RewardPackage> RewardPackages { get; set; }
        
        public IList<Post> Posts { get; set; }
        
        public IList<Media> Medias { get; set; }
        
        public Category Category { get; set; }
        
        public IList<UserProjectReward> UserProjectReward { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Project()
        {
            RewardPackages = new List<RewardPackage>();
            Posts = new List<Post>();
            Medias = new List<Media>();
        }
    }
}
