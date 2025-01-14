using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, TeamLead")]
    public class TeamController : BaseCrudController<TeamDto, ITeamRepository, ITeamService, TeamEditVM, TeamDetailsVM>
    {
        protected readonly IUserService _userService;
        protected readonly IProjectService _projectService;
        protected readonly ITeamService _teamService;


        public TeamController(IMapper mapper, ITeamService teamService, IProjectService projectService, IUserService userService)
            : base(teamService, mapper)
        {
            this._userService = userService;
            this._projectService = projectService;
            this._teamService = teamService;
        }

        protected override async Task<TeamEditVM> PrePopulateVMAsync(TeamEditVM editVM)
        {
            editVM.AllTeamLeaders = (await _userService.GetAllActiveAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()));
            editVM.Projects = (await _projectService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));

            return editVM;
        }
        [HttpGet]
        [Route("Team/List")]
        public async Task<IActionResult> List()
        {
            return await base.List();
        }
        [HttpGet]
        [Route("Team/Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(List));
            }

            var projects = await _projectService.GetAllAsync();
            var teams = await _teamService.GetAllAsync();

            teams = teams.Where(t =>
                t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.Project?.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true 
            ).ToList();

            var teamVMs = _mapper.Map<IEnumerable<TeamDetailsVM>>(teams);

            ViewBag.TeamVMs = teamVMs;

            return View("List", teamVMs); 
        }


        [HttpPost]
        public async Task<IActionResult> AddDeveloper(int teamId, int userId)
        {
            try
            {
                await _teamService.AddDeveloperToTeamAsync(teamId, userId);

                return RedirectToAction("Details", new { id = teamId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding developer: {ex.Message}");

                return StatusCode(500, "An error occurred while adding the developer.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoveDeveloper(int teamId, int userId)
        {
            try
            {
                // Премахване на разработчика чрез сървиса
                await _teamService.RemoveDeveloperFromTeamAsync(teamId, userId);

                // Пренасочване към детайлите на екипа
                return RedirectToAction("Details", new { id = teamId });
            }
            catch (Exception ex)
            {
                // Логване на грешката
                Console.WriteLine($"Error removing developer: {ex.Message}");

                // Връщане на грешка към клиента
                return StatusCode(500, "An error occurred while removing the developer.");
            }
        }


        [Route("Team/Details")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetByIdIfExistsAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            var availableDevelopers = (await _userService.GetAllAsync())
                .Where(user => user.Role != null && user.Role.Name == "Developer" && user.TeamId == null) 
                .Select(user => new { user.Id, user.FirstName, user.LastName })
                .ToList();

            ViewBag.Users = availableDevelopers;

            var teamVM = _mapper.Map<TeamDetailsVM>(team);

            return View(teamVM);
        }




    }

}
