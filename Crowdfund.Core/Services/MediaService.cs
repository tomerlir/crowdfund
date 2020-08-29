using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.MediaOptions;
using System;


namespace Crowdfund.Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly DataContext _context;

        public MediaService(DataContext context)
        {
            _context = context;
        }

        public Result<Media> CreateMedia(CreateMediaOptions options)
        {
            options.MediaUrl = options.MediaUrl?.Trim();
            if (string.IsNullOrWhiteSpace(options.MediaUrl))
            {
                return Result<Media>.Failed(StatusCode.BadRequest, "Empty media URL");
            }

            var url = options.MediaUrl.Trim();

            if (options.MediaType == (MediaType) MediaType.Video)
            {
                if (!url.Contains("youtube.com"))
                {
                    return Result<Media>.Failed(StatusCode.BadRequest, "Only youtube videos supported");
                }
            }
            
            if (options.MediaType == (MediaType) MediaType.Photo)
            {
                if (!url.Contains(".png") && !url.Contains(".jpg") && !url.Contains(".jpeg") && !url.Contains(".gif"))
                {
                    return Result<Media>.Failed(StatusCode.BadRequest, "Unsupported image type");
                }
            }

            var media = new Media
            {
                MediaUrl = url,
                MediaType = options.MediaType
            };

            _context.Add(media);

            return Result<Media>.Succeed(media);
        }

        public bool DeleteMedia(Media mediaToDelete)
        {
            _context.Remove(mediaToDelete);
            return _context.SaveChanges() > 0;
        }
    }
}