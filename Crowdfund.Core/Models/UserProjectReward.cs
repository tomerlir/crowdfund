using System;

namespace Crowdfund.Core.Models
{
    public class UserProjectReward
    {
        public int UserProjectRewardId { get; set; }
        
        public int UserId { get; set; }
        
        public int ProjectId { get; set; }
        
        public int? RewardPackageId { get; set; }
        
        public User User { get; set; }
        
        public Project Project { get; set; }
        
        public RewardPackage RewardPackage { get; set; }
        
        public bool IsOwner { get; set; }
        
        public int? Amount { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}