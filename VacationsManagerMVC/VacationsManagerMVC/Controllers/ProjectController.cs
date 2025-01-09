using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, TeamLead")]
    public class ProjectController : BaseCrudController<ProjectDto, IProjectRepository, IProjectService, ProjectEditVM, ProjectDetailsVM>
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService service, IMapper mapper, IProjectService projectService) : base(service, mapper)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("Project/List")]
        public async Task<IActionResult> List()
        {
            return await base.List();
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
    }
}
