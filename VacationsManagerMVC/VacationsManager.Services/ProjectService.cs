using System;
using System.Linq;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class ProjectService : BaseCrudService<ProjectDto, IProjectRepository>, IProjectService
    {
        private readonly ITeamService _teamService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;

        public ProjectService(IProjectRepository repository, ITeamService teamService, ITeamRepository teamRepository) : base(repository)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _projectRepository = repository;
            _teamRepository = teamRepository;
        }

        public async Task AddTeamToProjectAsync(int projectId, int teamId)
        {
            var project = await _repository.GetByIdAsync(projectId);
            if (project == null)
                throw new ArgumentException("Project not found.");

            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new ArgumentException("Team not found.");

            if (team.ProjectId == projectId)
                throw new InvalidOperationException("Team is already assigned to this project.");

            team.ProjectId = projectId;

            await _teamRepository.SaveAsync(team);
        }


        public async Task RemoveTeamFromProjectAsync(int projectId, int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new ArgumentException("Team not found.");

            if (team.ProjectId != projectId)
                throw new InvalidOperationException("Team is not assigned to this project.");

            team.ProjectId = null;

            await _teamRepository.SaveAsync(team);
        }

        public async Task<List<TeamDto>> GetAvailableTeamsAsync(int projectId)
        {
            return await _repository.GetAvailableTeamsAsync(projectId);
        }
    }
}
