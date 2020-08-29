using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.PostOptions;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IPostService
    {
        Result<Post> CreatePost(CreatePostOptions options);
        Result<Post> UpdatePost(Post post, UpdatePostOptions options);
        Result<Post> GetPostById(int? id);
        bool DeletePost(Post post);
    }
}
