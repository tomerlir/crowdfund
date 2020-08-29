namespace Crowdfund.Core.Models
{
    public class Media
    {
        public int MediaId { get; set; }
        
        public MediaType MediaType { get; set; }
        
        public string MediaUrl { get; set; }
    }
}