using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using VacationsManager.Data.Entities;
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
                .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.Id.ToString()));
            editVM.Projects = (await _projectService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));

            return editVM;
        }
        //[HttpGet]
        //[Route("Team/List")]
        //public async Task<IActionResult> List()
        //{
        //    return await base.List();
        //}
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


        [HttpGet]
        public override async Task<IActionResult> List(int pageSize = DefaultPageSize, int pageNumber = DefaultPageNumber)
        {
            var currentUserId = User.Identity.Name;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value; 

            if (string.IsNullOrEmpty(userRole))
            {
                return Unauthorized("User not found.");
            }

            if (userRole == "CEO")
            {
                return await base.List(pageSize, pageNumber);
            }

            var myTeams = await _teamService.GetTeamsByTeamLeadAsync(currentUserId, pageSize, pageNumber);
            var mappedTeams = _mapper.Map<IEnumerable<TeamDetailsVM>>(myTeams);

            return View(nameof(List), mappedTeams);
        }


        //[HttpPost]
        //public override async Task<IActionResult> Create(TeamEditVM editVM)
        //{
        //    try
        //    {
        //        // Записваме началото на операцията
        //        Console.WriteLine($"Започва създаването на екип с име: {editVM.Name}");

        //        // Проверка дали ID на лидера на екипа е валидно
        //        if (editVM.TeamLeaderId == 0)
        //        {
        //            Console.WriteLine("ID на лидер на екипа е 0. Ще се създаде нов лидер.");
        //        }

        //        // Присвояваме лидер на екипа и актуализираме потребителите
        //        var team = new Team
        //        {
        //            Name = editVM.Name,
        //            ProjectId = editVM.ProjectId,
        //            TeamLeaderId = editVM.TeamLeaderId
        //        };

        //        // Картографираме Team към TeamDto
        //        var teamDto = _mapper.Map<TeamDto>(team);

        //        // Присвояваме лидер на екипа и актуализираме потребителите
        //        await _teamService.CreateAndAssignTeamLeaderAsync(teamDto);

        //        // Успешно записване
        //        Console.WriteLine($"Екипът с име {editVM.Name} и лидер с ID {editVM.TeamLeaderId} е успешно създаден.");

        //        return RedirectToAction("List");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Логваме грешката
        //        Console.WriteLine($"Грешка при създаването на екипа с име {editVM.Name}. Грешка: {ex.Message}");

        //        // Може да предоставим на потребителя подробна информация в прозорец за грешка
        //        return View("Error");
        //    }
        //}




        [Route("Team/Details")]
        [HttpGet]
        public override async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetByIdIfExistsAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            var availableDevelopers = await _userService.GetAvailableDevelopersAsync();
            ViewBag.Users = availableDevelopers;

            var teamVM = _mapper.Map<TeamDetailsVM>(team);

            return View(teamVM);
        }


    }

}
