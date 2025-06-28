using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using YourNamespace.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Repos.Contracts
{
    public interface ITeamRepository : IBaseRepository<TeamDto>
    {
        //public Task CreateAndAssignTeamLeaderAsync(TeamDto teamDto);
        Task<IEnumerable<TeamDto>> GetTeamsByTeamLeadAsync(string teamLeadUsername, int pageSize, int pageNumber);
        Task AssignProjectToTeamAsync(int projectId, int teamId);
    }
}
