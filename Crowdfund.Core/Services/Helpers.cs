using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using System.Linq;

namespace Crowdfund.Core.Services
{
    public static class Helpers
    {
        public static bool UserOwnsProject(DataContext dbCtx ,int? userId, int? projectId)
        {
            return dbCtx.Set<UserProjectReward>()
                .Any(pj => pj.UserId == userId
                           && pj.ProjectId == projectId
                           && pj.IsOwner == true);
        }
        public static bool UserExists(DataContext dbCtx, int? userId)
        {
            return dbCtx.Set<User>().Any(u => u.UserId == userId);
        }
        
        public static bool ProjectExists(DataContext dbCtx ,int? projectId)
        {
            return dbCtx.Set<Project>().Any(p => p.ProjectId == projectId);
        }
        
        
        
    }
}