using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class ProjectRepository : BaseRepository<Project, ProjectDto>, IProjectRepository
    {
        public ProjectRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<List<TeamDto>> GetAvailableTeamsAsync(int projectId)
        {
            var project = await _context.Set<Project>()
                .Include(p => p.Teams)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project not found.");
            }

            var allTeams = await _context.Set<Team>().ToListAsync();

            var availableTeams = allTeams
                .Where(t => !project.Teams.Any(pt => pt.Id == t.Id))
                .ToList();

            return _mapper.Map<List<TeamDto>>(availableTeams);
        }
    }
}
