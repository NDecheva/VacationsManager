using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using YourNamespace.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Repos.Contracts
{
    public interface IProjectRepository : IBaseRepository<ProjectDto>
    {
        Task<List<TeamDto>> GetAvailableTeamsAsync(int projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsForTeamLeadAsync(string username, int pageSize, int pageNumber);
    }
}
