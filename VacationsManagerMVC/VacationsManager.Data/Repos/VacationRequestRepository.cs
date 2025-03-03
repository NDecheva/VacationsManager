using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class VacationRequestRepository : BaseRepository<VacationRequest, VacationRequestDto>, IVacationRequestRepository
    {
        public VacationRequestRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByUserRoleAsync(UserDto currentUser, RoleType role)
        {
            var requestsQuery = _context.Set<VacationRequest>()
                .AsNoTracking()
                .Include(r => r.Requester)
                .AsQueryable();

            switch (role)
            {
                case RoleType.Developer:
                    requestsQuery = requestsQuery.Where(r => r.RequesterId == currentUser.Id);
                    break;
                case RoleType.TeamLead:
                    var teamLeaderTeamId = await _context.Set<Team>()
                        .Where(t => t.TeamLeaderId == currentUser.Id)
                        .Select(t => t.Id)
                        .FirstOrDefaultAsync();

                    if (teamLeaderTeamId != 0)
                    {
                        requestsQuery = requestsQuery.Where(r =>
                            _context.Set<User>().Where(u => u.TeamId == teamLeaderTeamId)
                                .Select(u => u.Id).Contains(r.RequesterId) || r.RequesterId == currentUser.Id);
                    }
                    else
                    {
                        requestsQuery = requestsQuery.Where(r => r.RequesterId == currentUser.Id);
                    }
                    break;
                case RoleType.CEO:
                    break; // CEO вижда всички заявки
                default:
                    return new List<VacationRequestDto>();
            }

            return await requestsQuery
                .ProjectTo<VacationRequestDto>(_mapper.ConfigurationProvider)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByDateAsync(UserDto currentUser, RoleType role, DateTime startDate)
        {
            var allRequests = await GetRequestsByUserRoleAsync(currentUser, role);
            return allRequests.Where(r => r.StartDate >= startDate).ToList();
        }
    }
}
