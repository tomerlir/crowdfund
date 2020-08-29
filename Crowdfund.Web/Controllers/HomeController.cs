using System;
using System.Diagnostics;
using System.Linq;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.UserOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Http;
namespace Crowdfund.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IBackingService _backingService;
        private readonly IProjectService _projectService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IBackingService backingService, IProjectService projectService)
        {
            _logger = logger;
            _userService = userService;
            _backingService = backingService;
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Globals.UserId = HttpContext.Session.GetInt32("UserId");
            var trendingProjects = _backingService.TrendingProjects();

            if (!trendingProjects.Success)
            {
                return StatusCode((int)trendingProjects.ErrorCode,
                    trendingProjects.ErrorText);
            }
            var trendingProjectsToView = trendingProjects.Data.Select(p => new ProjectViewModel
            {
                UserName = _projectService.GetOwnerName(p.ProjectId),
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

            return View(trendingProjectsToView);           
        }
        
        [HttpPost]
        public IActionResult Index(CreateUserOptions userOptions) {
            var result = _userService.LoginUser(userOptions);
            
            if (!result.Success) {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }
            HttpContext.Session.SetInt32("UserId", result.Data);
            
            return Redirect("/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
