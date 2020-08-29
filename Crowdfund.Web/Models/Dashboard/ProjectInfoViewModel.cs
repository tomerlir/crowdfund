using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.PostOptions;
using Crowdfund.Core.Services.Options.RewardPackageOptions;

namespace Crowdfund.Web.Models.Dashboard
{
    public class ProjectInfoViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public CreateRewardPackageOptions RewardOptions { get; set; }
        public CreatePostOptions PostOptions { get; set; }
    }
}