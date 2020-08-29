using Crowdfund.Core.Models;
using System.Collections.Generic;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IBackingService
    {
        Result<bool> CreateBacking(int? userId, int? projectId, int rewardPackageId, int? amount);
        
        
        Result<int> GetProjectBackingsAmount(int? projectId);

        Result<IEnumerable<Project>> GetUserProjects(int? userId);

        Result<IEnumerable<Project>> GetBackedProjects(int? backerId);

        Result<int> GetProjectBackingsCount(int? projectId);

        Result<IEnumerable<Project>> TrendingProjects();
    }
}