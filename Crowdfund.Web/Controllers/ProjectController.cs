using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.BackingOptions;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("projects")]
    public class ProjectController : Controller
    {
        private readonly DataContext _context;

        private readonly IProjectService _projectService;
        private readonly IBackingService _backingService;


        public ProjectController(DataContext context, IProjectService projectService, IBackingService backingService)
        {
            _context = context;
            _projectService = projectService;
            _backingService = backingService;
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var project = _projectService.GetProjectById(id);

            if (!project.Success)
            {
                return StatusCode((int) project.ErrorCode,
                    project.ErrorText);
            }

            var projectToView = new DetailsViewModel
            {
                UserName = _projectService.GetOwnerName(project.Data.ProjectId),
                ProjectId = project.Data.ProjectId,
                Title = project.Data.Title,
                Description = project.Data.Description,
                Category = project.Data.Category,
                DaysToGo = (project.Data.DueTo - DateTime.Now).Days,
                Goal = project.Data.Goal,
                MainImageUrl = project.Data.MainImageUrl,
                Medias = project.Data.Medias,
                Posts = project.Data.Posts.OrderByDescending(p => p.CreatedAt),
                RewardPackages = project.Data.RewardPackages.OrderBy(p => p.MinAmount),
                IsFirstImage = true,
                Backings = _backingService.GetProjectBackingsCount(id).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(id).Data,
                Progress =
                    (int) ((decimal) _backingService.GetProjectBackingsAmount(id).Data / project.Data.Goal * 100),
                InterestingProjects = _projectService.GetAllProjects().Data.Where(p => p.ProjectId != id)
                    .OrderBy(x => Guid.NewGuid()).Take(3)
                    .Select(p => new InterestingProject
                    {
                        ProjectId = p.ProjectId,
                        Category = p.Category,
                        DaysToGo = (p.DueTo - DateTime.Now).Days,
                        MainImageUrl = p.MainImageUrl,
                        Title = p.Title,
                        UserName = _projectService.GetOwnerName(p.ProjectId)
                    })
            };


            return View(projectToView);
        }

        [HttpPost]
        public IActionResult Back([FromBody] CreateBackingOptions options)
        {
            var backResult = _backingService.CreateBacking(Globals.UserId, options.ProjectId,
                options.RewardPackageId, options.Amount);
            if (!backResult.Success)
            {
                return StatusCode((int) backResult.ErrorCode,
                    backResult.ErrorText);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult All()
        {
            var allProject = _projectService.GetAllProjects();

            if (!allProject.Success)
            {
                return StatusCode((int) allProject.ErrorCode,
                    allProject.ErrorText);
            }

            var projectsToView = allProject.Data.Select(p => new ProjectViewModel
            {
                 UserName = _projectService.GetOwnerName(p.ProjectId),
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DaysToGo = (p.DueTo - DateTime.Now).Days,
                Backings = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Category=p.Category, 
                Goal = p.Goal,
                Progress = (int) ((decimal) _backingService.GetProjectBackingsAmount(p.ProjectId).Data / p.Goal * 100)
            });

            return View(projectsToView);
        }

        [HttpGet("explore")]
        public IActionResult SearchByQueryString(string q)
        {
            var results = string.IsNullOrWhiteSpace(q) ? null :  _projectService.SearchProjects(new SearchProjectOptions
            {
                SearchString = q
            }).ToList();

            var projectsToView = results?.Select(p => new ProjectViewModel
            {
                UserName = _projectService.GetOwnerName(p.ProjectId),
                ProjectId = p.ProjectId,
                Title = p.Title,
                Category = p.Category,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DaysToGo = (p.DueTo - DateTime.Now).Days,
                Backings = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress = (int) ((decimal) _backingService.GetProjectBackingsAmount(p.ProjectId).Data / p.Goal * 100)
            });
            
            return View(projectsToView);
        }

        [HttpGet("explore/category/{category}")]
        public IActionResult SearchByCategoryResults(Category category)
        {
            var results = _projectService.SearchProjects(new SearchProjectOptions
            {
                SingleCategoryId = (int) category,
            }).ToList();

            var projectsToView = results.Select(p => new ProjectViewModel
            {
                UserName = _projectService.GetOwnerName(p.ProjectId),
                ProjectId = p.ProjectId,
                Title = p.Title,
                Category = p.Category,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DaysToGo = (p.DueTo - DateTime.Now).Days,
                Backings = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress = (int) ((decimal) _backingService.GetProjectBackingsAmount(p.ProjectId).Data / p.Goal * 100)
            });

            ViewBag.Category = category;
            return View(projectsToView);
        }
    }
}