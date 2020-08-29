using System;
using Crowdfund.Core.Models;

namespace Crowdfund.Web.Models
{
    public class ProjectViewModel
    {
        public string UserName { get; set; }
        public int ProjectId { get; set; }

        public string Title { get; set; }
        
        public Category Category { get; set; }

        public string Description { get; set; }

        public int DaysToGo { get; set; }

        public string MainImageUrl { get; set; }

        public int Goal { get; set; }
        
        public int Backings { get; set; }

        public int BackingsAmount { get; set; }

        public int Progress { get; set; }
        
    }
}