using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.RewardPackageOptions;

namespace Crowdfund.Core.Services
{
    public class RewardService : IRewardService
    {
        private readonly DataContext _context;

        public RewardService(DataContext context)
        {
            _context = context;
        }

        public Result<RewardPackage> CreateRewardPackage(CreateRewardPackageOptions options)
        {
            if(options == null)
            {
                return Result<RewardPackage>.Failed(StatusCode.BadRequest, "Please fill in the form");
            }

            options.Title = options.Title?.Trim();
            options.Description = options.Description?.Trim();
            
            if (string.IsNullOrWhiteSpace(options.Title))
            {
                return Result<RewardPackage>.Failed(StatusCode.BadRequest, "Please enter a Title");
            }

            if(options.Quantity < 0)
            {
                return Result<RewardPackage>.Failed(StatusCode.BadRequest, "Please enter a valid Quantity");
            }

            if(options.MinAmount <= 0 || options.MinAmount == null)
            {
                return Result<RewardPackage>.Failed(StatusCode.BadRequest, "Please enter a valid Amount");
            }
            
            var reward = new RewardPackage
            {
                Title = options.Title,
                Description = options.Description,
                MinAmount = options.MinAmount,
                Quantity = options.Quantity
            };

            return Result<RewardPackage>.Succeed(reward);
        }
       
        public RewardPackage UpdateRewardPackage(RewardPackage packageToUpdate ,UpdateRewardPackageOptions options)
        {
            
            options.Title = options.Title?.Trim();
            options.Description = options.Description?.Trim();

            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                packageToUpdate.Title = options.Title;
            }

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                packageToUpdate.Description = options.Description;
            }

            if (options.Quantity != null)
            {
                packageToUpdate.Quantity = options.Quantity;
            }

            if (options.MinAmount != null || options.MinAmount <= 0)
            {
                packageToUpdate.MinAmount = options.MinAmount.Value;
            }
        
            return packageToUpdate;
        }

        public bool DeleteRewardPackage(RewardPackage rewardPackage)
        {
            _context.Remove(rewardPackage);

            return _context.SaveChanges() > 0;
        }

        public RewardPackage GetRewardPackageById(int? packageId)
        {
            return packageId == null ? null : _context.Set<RewardPackage>().Find(packageId);
        }
    }
}