using System;

namespace Crowdfund.Core.Services.Options.ProjectOptions
{
    public class CreateProjectOptions
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        public string MainImageUrl { get; set; }

        public DateTime? DueTo { get; set; }

        public int Goal { get; set; }
        
        public int CategoryId { get; set;}
    }
}