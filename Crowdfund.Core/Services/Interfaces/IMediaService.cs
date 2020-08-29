using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.MediaOptions;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IMediaService
    {
        Result<Media> CreateMedia(CreateMediaOptions options);
        bool DeleteMedia(Media media);
    }
}
