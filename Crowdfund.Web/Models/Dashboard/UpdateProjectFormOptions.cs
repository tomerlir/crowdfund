using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfund.Web.Models.Dashboard
{
    public class UpdateProjectFormOptions
    {
        public int? ProjectId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string MainImageUrl { get; set; }

        public DateTime DueTo { get; set; }

        public int Goal { get; set; }

        public int CategoryId { get; set; }
    }
}
