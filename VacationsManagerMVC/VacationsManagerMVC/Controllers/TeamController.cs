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

    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, TeamLead")]
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

            // Ако не е подаден searchTerm, пренасочваме към списъка с всички проекти
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(List));
            }

            // Получаваме всички проекти и екипи
            var projects = await _projectService.GetAllAsync();
            var teams = await _teamService.GetAllAsync();

            // Филтрираме проектите и екипите на базата на търсенето
            projects = projects.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            teams = teams.Where(t => t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Мапваме проектите и екипите към ViewModel
            var projectVMs = _mapper.Map<IEnumerable<ProjectDetailsVM>>(projects);
            var teamVMs = _mapper.Map<IEnumerable<TeamDetailsVM>>(teams);

            // Записваме в ViewBag или ViewData
            ViewBag.ProjectVMs = projectVMs;
            ViewBag.TeamVMs = teamVMs;
           
            // Връщаме изгледа с филтрираните данни
            return View("List");
        }


    }

}
