using Crowdfund.Core.Models;

namespace Crowdfund.Core.Services.Options.MediaOptions
{
    public class CreateMediaOptions
    {
        public MediaType MediaType { get; set; }
        public string MediaUrl { get; set; }
    }
}