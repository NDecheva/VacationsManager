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
            IQueryable<VacationRequest> query = _dbSet
                .AsNoTracking()
                .Include(r => r.Requester)
                .ThenInclude(u => u.Role)
                .Include(r => r.Requester)
                .ThenInclude(u => u.Team);

            switch (role)
            {
                case RoleType.Developer:
                    query = query.Where(r => r.RequesterId == currentUser.Id);
                    break;

                case RoleType.TeamLead:
                    int? teamId = await _context.Set<Team>()
                        .Where(t => t.TeamLeaderId == currentUser.Id)
                        .Select(t => (int?)t.Id)
                        .FirstOrDefaultAsync();

                    if (teamId.HasValue)
                    {
                        var userIds = await _context.Set<User>()
                            .Where(u => u.TeamId == teamId.Value)
                            .Select(u => u.Id)
                            .ToListAsync();

                        query = query.Where(r => userIds.Contains(r.RequesterId) || r.RequesterId == currentUser.Id);
                    }
                    else
                    {
                        query = query.Where(r => r.RequesterId == currentUser.Id);
                    }
                    break;

                case RoleType.CEO:
                    // без филтри
                    break;

                default:
                    return Enumerable.Empty<VacationRequestDto>();
            }

            var entities = await query.ToListAsync();
            return MapToEnumerableOfModel(entities);
        }


        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByDateAsync(UserDto currentUser, RoleType role, DateTime startDate)
        {
            var allRequests = await GetRequestsByUserRoleAsync(currentUser, role);
            return allRequests.Where(r => r.StartDate >= startDate).ToList();
        }
    }
}
