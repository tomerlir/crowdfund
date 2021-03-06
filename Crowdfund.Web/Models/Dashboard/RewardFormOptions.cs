namespace Crowdfund.Web.Models.Dashboard
{
    public class RewardFormOptions
    {
        public int? ProjectId { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public int? MinAmount { get; set; }

        public int? Quantity { get; set; }
    }
}