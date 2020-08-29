using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Crowdfund.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Address { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public IList<UserProjectReward> UserProjectReward { get; set; }

        public User()
        {
            UserProjectReward = new List<UserProjectReward>();
        }

        public bool IsValidEmail(String Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(Email, expression))
                {
                    if (Regex.Replace(Email, expression, string.Empty).Length == 0)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
    }
}