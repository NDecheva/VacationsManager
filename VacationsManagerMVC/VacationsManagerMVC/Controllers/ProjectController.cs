using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    public class ProjectController : BaseCrudController<ProjectDto, IProjectRepository, IProjectService, ProjectEditVM, ProjectDetailsVM>
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService service, IMapper mapper, IProjectService projectService) : base(service, mapper)
        {
            _projectService = projectService;
        }
    }
}
