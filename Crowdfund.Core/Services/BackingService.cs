using System;
using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Core.Services
{
    public class BackingService : IBackingService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public BackingService(DataContext context, IUserService userService, IProjectService projectService)
        {
            _context = context;
            _userService = userService;
            _projectService = projectService;
        }

        public Result<bool> CreateBacking(int? userId, int? projectId, int rewardPackageId, int? amount)
        {
            if (userId == null || projectId == null ) return Result<bool>.Failed(StatusCode.BadRequest, "Null options");

            var user = _userService.GetUserById(userId);
            var project = _projectService.GetProjectById(projectId);

            if (user == null || project == null)
                return Result<bool>.Failed(StatusCode.NotFound, "User or Project Not Found");

            if (amount <= 0 || amount == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Please enter a valid amount");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId))
                return Result<bool>.Failed(StatusCode.BadRequest, "You cannot back your own project");

            var rewardPackage = rewardPackageId != 0 ? project.Data.RewardPackages
                .FirstOrDefault(rp => amount >= rp.MinAmount && rp.RewardPackageId == rewardPackageId) : null;

            if(rewardPackage != null && rewardPackageId != 0)
            {
                if(rewardPackage.Quantity == 0)
                {
                    return Result<bool>.Failed(StatusCode.BadRequest, "Reward Package No Longer Available");
                }
                rewardPackage.Quantity--;
            }

            if (rewardPackage == null && rewardPackageId != 0)
                return Result<bool>.Failed(StatusCode.NotFound, "Not eligible for this reward package");

            var userProjectBacking = new UserProjectReward
            {
                IsOwner = false,
                Project = project.Data,
                RewardPackage = rewardPackageId == 0 ? null : rewardPackage,
                Amount = amount
            };

            user.UserProjectReward.Add(userProjectBacking);

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Backing could not be created")
                : Result<bool>.Succeed(true);
        }


        public Result<IEnumerable<Project>> GetUserProjects(int? userId)
        {
            if (userId == null) return Result<IEnumerable<Project>>.Failed(StatusCode.BadRequest, "Null options");

            var user = Helpers.UserExists(_context, userId);

            if (user == false) return Result<IEnumerable<Project>>.Failed(StatusCode.NotFound, "User Not Found");

            try
            {
                var projects = _context.Set<UserProjectReward>()
                    .Include(p => p.Project)
                    .Where(u => u.ProjectId == u.Project.ProjectId && u.UserId == userId && u.IsOwner == true)
                    .Select(p => p.Project).ToList();

                return Result<IEnumerable<Project>>.Succeed(projects);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Project>>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public Result<IEnumerable<Project>> GetBackedProjects(int? backerId)
        {
            if (backerId == null) return Result<IEnumerable<Project>>.Failed(StatusCode.BadRequest, "Null options");

            var backer = Helpers.UserExists(_context, backerId);

            if (backer == false) return Result<IEnumerable<Project>>.Failed(StatusCode.NotFound, "User Not Found");

            try
            {
                var projects = _context.Set<UserProjectReward>()
                    .Include(p => p.Project)
                    .Where(u => u.ProjectId == u.Project.ProjectId && u.UserId == backerId && u.IsOwner == false)
                    .Select(p => p.Project).Distinct().ToList();

                return Result<IEnumerable<Project>>.Succeed(projects);
                
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Project>>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }


        public Result<int> GetProjectBackingsAmount(int? projectId)
        {
            if (projectId == null) return Result<int>.Failed(StatusCode.BadRequest, "Null option");

            if (Helpers.ProjectExists(_context, projectId) == false)
            {
                return Result<int>.Failed(StatusCode.NotFound, "Project Not Found");
            }

            try
            {
                var backingsAmount = _context.Set<UserProjectReward>()
                    .Where(u => u.ProjectId == projectId && u.IsOwner == false)
                    .Sum(a => a.Amount);

                return Result<int>.Succeed(backingsAmount ?? 0);
            }
            catch (Exception ex)
            {
                return Result<int>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public Result<int> GetProjectBackingsCount(int? projectId)
        {
            if (projectId == null) return Result<int>.Failed(StatusCode.BadRequest, "Null option");

            try
            {
                var backings = _context.Set<UserProjectReward>()
                    .Count(u => u.ProjectId == projectId && u.IsOwner == false);

                return Result<int>.Succeed(backings);
            }
            catch (Exception ex)
            {
                return Result<int>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public Result<IEnumerable<Project>> TrendingProjects()
        {
            try
            {
                var projectBakings = _context.Set<UserProjectReward>()
                    .Where(u => u.IsOwner == false)
                    .GroupBy(p => new { p.ProjectId })
                    .Select(p => new { p.Key.ProjectId, Sum = p.Sum(s => s.Amount) })
                    .OrderByDescending(o => o.Sum).Select(p => p.ProjectId)
                    .Take(10).ToList();

                return Result<IEnumerable<Project>>
                    .Succeed(projectBakings.Select(id => _projectService.GetSingleProject(id).Data));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Project>>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }
    }
}