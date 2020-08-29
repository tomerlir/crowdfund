namespace Crowdfund.Core.Models
{
    public class RewardPackage
    {
        public int RewardPackageId { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; } 
        
        public int? MinAmount { get; set; } 
        
        public int? Quantity { get; set; }
    }
}
