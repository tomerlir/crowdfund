using System;
using System.Linq;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.MediaOptions;
using Crowdfund.Core.Services.Options.PostOptions;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Core.Services.Options.RewardPackageOptions;
using Crowdfund.Core.Services.Options.UserOptions;
using Crowdfund.Web.Models;
using Crowdfund.Web.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("Dashboard/User")]
    public class DashboardController : Controller
    {
        private readonly IBackingService _backingService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public DashboardController(IBackingService backingService,
            IProjectService projectService, IUserService userService)
        {
            _backingService = backingService;
            _projectService = projectService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            Globals.HasError = null;
            var projects = _backingService.GetUserProjects(id);

            if (!projects.Success)
            {
                return StatusCode((int)projects.ErrorCode,
                    projects.ErrorText);
            }

            var projectsToView = projects.Data.Select(p => new ProjectViewModel
            {
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                MainImageUrl = p.MainImageUrl,
                DaysToGo = (p.DueTo - DateTime.Now).Days,
                Backings = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress = (int)((decimal)_backingService.GetProjectBackingsAmount(p.ProjectId).Data / p.Goal * 100)
            });

            return View(projectsToView);
        }



        [HttpGet("{id}/backed")]
        public IActionResult BackedProjects(int id)
        {
            var projects = _backingService.GetBackedProjects(id);

            if (!projects.Success)
            {
                return StatusCode((int)projects.ErrorCode,
                    projects.ErrorText);
            }

            var projectsToView = projects.Data.Select(p => new ProjectViewModel
            {
                UserName = _projectService.GetOwnerName(p.ProjectId),
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                MainImageUrl = p.MainImageUrl,
                DaysToGo = (p.DueTo - DateTime.Now).Days,
                Backings = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = (int)_backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress = (int)((decimal)_backingService.GetProjectBackingsAmount(p.ProjectId).Data / p.Goal * 100)
            });

            return View(projectsToView);
        }



        [HttpGet]
        [Route("project/create")]
        public IActionResult CreateProject()
        {

            return View();
        }


        [HttpPost]
        [Route("project/create")]
        public IActionResult CreateProject([FromBody] CreateProjectOptions options)
        {
            var result = _projectService.CreateProject(Globals.UserId, options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }
            
            return Ok();
        }



        [HttpGet("post/project/{id}")]
        public IActionResult CreatePost(int id)
        {
            var projectTitle = _projectService.GetSingleProject(id).Data.Title;
            var projectInfoViewModel = new ProjectInfoViewModel
            {
                ProjectId = id,
                ProjectTitle = projectTitle
            };

            return View(projectInfoViewModel);
        }

        [HttpPost]
        [Route("post/project/{id}")]
        public IActionResult CreatePost([FromBody] PostFormOptions options)
        {
            var postOptions = new CreatePostOptions
            {
                Title = options.Title,
                Text = options.Text
            };

            var result = _projectService.AddPost(postOptions, Globals.UserId, options.ProjectId);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }
            
            return Ok();
        }



        [HttpGet("reward/project/{id}")]
        public IActionResult CreateRewardPackage(int id)
        {
            var projectTitle = _projectService.GetSingleProject(id).Data.Title;
            var projectInfoViewModel = new ProjectInfoViewModel
            {
                ProjectId = id,
                ProjectTitle = projectTitle,
                RewardOptions = new CreateRewardPackageOptions(),
                PostOptions = new CreatePostOptions()
            };
            
            return View(projectInfoViewModel);
        }

        [HttpPost]
        [Route("reward/project/{id}")]
        public IActionResult CreateRewardPackage([FromBody] RewardFormOptions options)
        {
            var rewardPackageOptions = new CreateRewardPackageOptions
            {
                Title = options.Title,
                Description = options.Description,
                MinAmount = options.MinAmount,
                Quantity = options.Quantity
            };

            var result = _projectService.AddRewardPackage(options.ProjectId, Globals.UserId, rewardPackageOptions);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }

            return Ok();
        }



        [HttpGet("images/project/{id}")]
        public IActionResult AddImages(int id)
        {
            var projectTitle = _projectService.GetSingleProject(id).Data.Title;
            var projectInfoViewModel = new ProjectInfoViewModel
            {
                ProjectId = id,
                ProjectTitle = projectTitle
            };

            return View(projectInfoViewModel);
        }

        [HttpPost]
        [Route("images/project/{id}")]
        public IActionResult AddImages(MediaFormOptions options)
        {
            var urlList = options.Url.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            var createMediaOptions = urlList.Select
            (url => new CreateMediaOptions
            {
                MediaType = MediaType.Photo,
                MediaUrl = url,
            });

            var result = _projectService.AddMedia(createMediaOptions, Globals.UserId, options.ProjectId);

            if (!result.Success)
            {
                Globals.HasError = true;
                Globals.Error = result.ErrorText;
            }
            else
            {
                Globals.HasError = false;
                Globals.Error = null;
            }

            return RedirectToAction("AddImages", options.ProjectId);
        }

        [HttpGet("videos/project/{id}")]
        public IActionResult AddVideos(int id)
        {
            var projectTitle = _projectService.GetSingleProject(id).Data.Title;
            var projectInfoViewModel = new ProjectInfoViewModel
            {
                ProjectId = id,
                ProjectTitle = projectTitle
            };

            return View(projectInfoViewModel);
        }

        [HttpPost]
        [Route("videos/project/{id}")]
        public IActionResult AddVideos(MediaFormOptions options)
        {
            var urlList = options.Url.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            
            var createMediaOptions = urlList.Select
            (url => new CreateMediaOptions
            {
                MediaType = MediaType.Video,
                MediaUrl = url,
            });

            var result = _projectService.AddMedia(createMediaOptions, Globals.UserId, options.ProjectId);

            if (!result.Success)
            {
                Globals.HasError = true;
                Globals.Error = result.ErrorText;
            }
            else
            {
                Globals.HasError = false;
                Globals.Error = null;
            }

            return RedirectToAction("AddVideos", options.ProjectId);
        }



        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult UpdateUser(int id)
        {
            var user = _userService.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult UpdateUser([FromBody] UpdateUserOptions options)
        {
            var result = _userService.UpdateUser(Globals.UserId, options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }

            return Ok();
        }



        [HttpGet]
        [Route("project/edit/{id}")]
        public IActionResult UpdateProject(int id)
        {
            var project = _projectService.GetSingleProject(id).Data;

            return View(project);
        }

        [HttpPost]
        [Route("project/edit/{id}")]
        public IActionResult UpdateProject([FromBody] UpdateProjectFormOptions options)
        {
            var editProjectOptions = new UpdateProjectOptions()
            {
                ProjectId = options.ProjectId,
                Title = options.Title,
                Description = options.Description,
                MainImageUrl = options.MainImageUrl,
                DueTo = options.DueTo,
                Goal = options.Goal,
                CategoryId = options.CategoryId
            };

            var result = _projectService.UpdateProject
                (Globals.UserId, editProjectOptions.ProjectId, editProjectOptions);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }
            return Ok();
        }
    }
}