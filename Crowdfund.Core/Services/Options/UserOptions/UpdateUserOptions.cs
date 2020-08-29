using Crowdfund.Core.Models;
using System.Collections.Generic;


namespace Crowdfund.Core.Services.Options.UserOptions
{
    public class UpdateUserOptions
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
       // public IList<UserProjectReward> UserProjectReward { get; set; }

        //public UpdateUserOptions()
        //{
        //    UserProjectReward = new List<UserProjectReward>();
        //}
    }
}
