using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface IProjectService : IBaseCrudService<ProjectDto, IProjectRepository>

    {
        public Task AddTeamToProjectAsync(int projectId, int teamId);
        public Task RemoveTeamFromProjectAsync(int projectId, int teamId);
    }
}
