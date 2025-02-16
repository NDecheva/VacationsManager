using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, TeamLead")]
    public class ProjectController : BaseCrudController<ProjectDto, IProjectRepository, IProjectService, ProjectEditVM, ProjectDetailsVM>
    {
        private readonly ITeamService _teamService;
        private readonly IProjectService _projectService;

        public ProjectController(IMapper mapper, IProjectService projectService, ITeamService teamService)
            : base(projectService, mapper)
        {
            _teamService = teamService;
            _projectService = projectService;
        }


        [HttpPost]
        public override async Task<IActionResult> Create(ProjectEditVM editVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editVM);
            }

            var projectDto = _mapper.Map<ProjectDto>(editVM);
            var currentUser = User.Identity.Name; 

            await _service.CreateProjectAsync(projectDto, currentUser); 

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public override async Task<IActionResult> List(int pageSize = DefaultPageSize, int pageNumber = DefaultPageNumber)
        {
            var currentUserId = User.Identity.Name;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value; 

            if (string.IsNullOrEmpty(userRole))
            {
                return Unauthorized("User not found.");
            }

            if (userRole != "TeamLead")
            {
                return await base.List(pageSize, pageNumber);
            }

            var projects = await _projectService.GetProjectsForTeamLeadAsync(currentUserId, pageSize, pageNumber);
            var mappedProjects = _mapper.Map<IEnumerable<ProjectDetailsVM>>(projects);

            return View(nameof(List), mappedProjects);
        }



        [HttpGet]
        [Route("Project/Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(List));
            }

            var projects = await _projectService.GetAllAsync();

            projects = projects.Where(p =>
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            var projectVMs = _mapper.Map<IEnumerable<ProjectDetailsVM>>(projects);

            return View("List", projectVMs);
        }


        [HttpPost]
        public async Task<IActionResult> AddTeam(int projectId, int teamId)
        {
            try
            {
                await _projectService.AddTeamToProjectAsync(projectId, teamId);
                return RedirectToAction("Details", new { id = projectId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding team: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the team.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTeam(int projectId, int teamId)
        {
            try
            {
                await _projectService.RemoveTeamFromProjectAsync(projectId, teamId);
                return RedirectToAction("Details", new { id = projectId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing team: {ex.Message}");
                return StatusCode(500, "An error occurred while removing the team.");
            }
        }

        [HttpGet]
        [Route("Project/Details")]
        public override async Task<IActionResult> Details(int id)
        {
            var baseResult = await base.Details(id) as ViewResult;

            if (baseResult == null || baseResult.Model == null)
            {
                return NotFound("Project not found.");
            }

            var projectVM = baseResult.Model as ProjectDetailsVM;

            if (projectVM == null)
            {
                return BadRequest("Unable to load project details.");
            }

            // Извикване на сървиса за наличните екипи
            var availableTeams = await _projectService.GetAvailableTeamsAsync(id);

            // Задаване на ViewBag.Teams
            ViewBag.Teams = availableTeams.Select(team => new SelectListItem
            {
                Value = team.Id.ToString(),
                Text = team.Name
            }).ToList();

            return View(projectVM);
        }



    }
}
