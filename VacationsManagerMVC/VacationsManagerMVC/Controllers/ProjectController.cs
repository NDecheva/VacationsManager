using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //[HttpGet]
        //[Route("Project/List")]
        //public async Task<IActionResult> List()
        //{
        //    return await base.List();
        //}

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
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdIfExistsAsync(id);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            // Зареждане на всички екипи
            var allTeams = await _teamService.GetAllAsync();
            // Намиране на наличните екипи, които не са асоциирани с проекта
            var availableTeams = allTeams.Where(t => !project.Teams.Any(pt => pt.Id == t.Id)).ToList();

            // Задаване на ViewBag.Teams за използване в изгледа
            ViewBag.Teams = availableTeams.Select(team => new SelectListItem
            {
                Value = team.Id.ToString(),
                Text = team.Name
            }).ToList();

            // Мапване на проекта към ViewModel
            var projectVM = _mapper.Map<ProjectDetailsVM>(project);
            return View(projectVM);
        }


    }
}
