using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options.BackingOptions
{
    public class CreateBackingOptions
    {
        public int ProjectId { get; set; }
        public int RewardPackageId { get; set; }
        public int? Amount { get; set; }
    }
}
