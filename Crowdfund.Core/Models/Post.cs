using System;

namespace Crowdfund.Core.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}