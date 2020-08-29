using System.Collections.Generic;

namespace Crowdfund.Web.Models.Dashboard
{
    public class MediaFormOptions
    {
        public int ProjectId { get; set; }
        public IEnumerable<string> Url { get; set; }
    }
}