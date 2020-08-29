using Crowdfund.Core.Models;

namespace Crowdfund.Web.Models
{
    public class InterestingProject
    {
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }

        public int DaysToGo { get; set; }

        public string MainImageUrl { get; set; }

        public Category Category { get; set; }
    }
}