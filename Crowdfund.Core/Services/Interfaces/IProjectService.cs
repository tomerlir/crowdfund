using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.MediaOptions;
using Crowdfund.Core.Services.Options.PostOptions;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Core.Services.Options.RewardPackageOptions;
using System.Collections.Generic;
using System.Linq;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IProjectService
    {
        Result<bool> CreateProject(int? userId, CreateProjectOptions createProjectOptions);
        
        Result<Project> GetProjectById(int? id);
        
        Result<Project> GetSingleProject(int? id);

        Result<IEnumerable<Project>> GetAllProjects();

        Result<bool> UpdateProject(int? userId, int? projectId, UpdateProjectOptions updateProjectOptions);

        IQueryable<Project> SearchProjects(SearchProjectOptions searchProjectOptions);

        Result<bool> DeleteProject(int? userId, int? projectId);

        Result<bool> AddRewardPackage(int? projectId, int? userId, CreateRewardPackageOptions createRewardPackageOptions);

        Result<bool> UpdateRewardPackage(int? projectId, int? userId, int? rewardPackageId, UpdateRewardPackageOptions updateRewardPackageOptions);

        Result<bool> DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId);

        Result<bool> AddMedia(IEnumerable<CreateMediaOptions> createMediaOptions, int? userId, int? projectId);

        Result<bool> DeleteMedia(int? userId, int? projectId, int? mediaId);

        IList<Media> GetProjectPhotos(int? projectId);

        IList<Media> GetProjectVideos(int? projectId);
        
        IList<Post> GetProjectPosts(int? projectId);

        Result<bool> AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId);

        Result<bool> UpdatePost(int? postId, int? userId, int? projectId, UpdatePostOptions updatePostOptions);

        Result<bool> DeletePost(int? postId, int? userId, int? projectId);
        
        string GetOwnerName(int? projectId);

    }
}