using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class TeamRepository : BaseRepository<Team, TeamDto>, ITeamRepository
    {
        public TeamRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}