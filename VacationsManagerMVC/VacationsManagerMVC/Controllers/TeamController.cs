using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin, Employee, User")]
    public class TeamController : BaseCrudController<TeamDto, ITeamRepository, ITeamService, TeamEditVM, TeamDetailsVM>
    {
        protected readonly ITeamService _teamService;
        protected readonly IProjectService _projectService;


        public TeamController(IMapper mapper, ITeamService teamService, IProjectService projectService)
            : base(teamService, mapper)
        {
            this._teamService = teamService;
            this._projectService = projectService;
        }

        protected override async Task<TeamEditVM> PrePopulateVMAsync(TeamEditVM editVM)
        {
            editVM.AllTeamLeaders = (await _teamService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));
            editVM.Projects = (await _teamService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));
            

            return editVM;
        }
    }

}
