using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface ITeamService : IBaseCrudService<TeamDto, ITeamRepository>

    {
        public Task AddDeveloperToTeamAsync(int teamId, int userId);
        public Task RemoveDeveloperFromTeamAsync(int teamId, int userId);

        //public Task CreateAndAssignTeamLeaderAsync(TeamDto teamDto);
    }
}
