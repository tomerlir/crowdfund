using Crowdfund.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfund.Web.Models
{
    public class DetailsViewModel
    {
        public string UserName { get; set; }
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DaysToGo { get; set; }

        public string MainImageUrl { get; set; }

        public decimal Goal { get; set; }

        public Category Category { get; set; }

        public int Backings { get; set; }

        public decimal BackingsAmount { get; set; }
        public int Progress { get; set; }
        public bool IsFirstImage { get; set; } 
        public IEnumerable<RewardPackage> RewardPackages { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Media> Medias { get; set; }

        public IEnumerable<InterestingProject> InterestingProjects { get; set; }
    }
}
