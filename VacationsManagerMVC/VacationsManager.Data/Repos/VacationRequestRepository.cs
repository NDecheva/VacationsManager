using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static VacationsManager.Data.Repos.VacationRequestRepository;
using VacationsManager.Shared.Attributes;
using YourNamespace.Data.Repos;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Enums;
using AutoMapper.QueryableExtensions;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class VacationRequestRepository : BaseRepository<VacationRequest, VacationRequestDto>,
        IVacationRequestRepository
    {
        public VacationRequestRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByUserRoleAsync(UserDto currentUser, RoleType role)
        {
            var requestsQuery = _context.Set<VacationRequest>()
                .AsNoTracking() // По-бързо зареждане без проследяване на промените
                .Include(r => r.Requester) // Включване на Requester
                .AsQueryable();

            // Филтриране по роля на потребителя
            if (role == RoleType.Developer)
            {
                requestsQuery = requestsQuery.Where(r => r.RequesterId == currentUser.Id);
            }
            else if (role == RoleType.TeamLead)
            {
                // Вземаме заявки на членовете на екипа на TeamLead
                var teamLeaderTeamId = await _context.Set<Team>()
                    .Where(t => t.TeamLeaderId == currentUser.Id)
                    .Select(t => t.Id)
                    .FirstOrDefaultAsync();

                if (teamLeaderTeamId != 0)
                {
                    var teamMembersIds = await _context.Set<User>()
                        .Where(u => u.TeamId == teamLeaderTeamId)
                        .Select(u => u.Id)
                        .ToListAsync();

                    requestsQuery = requestsQuery.Where(r => teamMembersIds.Contains(r.RequesterId) || r.RequesterId == currentUser.Id);
                }
                else
                {
                    requestsQuery = requestsQuery.Where(r => r.RequesterId == currentUser.Id);
                }
            }

            // Мапираме резултатите от VacationRequest към VacationRequestDto чрез AutoMapper
            var result = await requestsQuery
                .ProjectTo<VacationRequestDto>(_mapper.ConfigurationProvider) // Използваме AutoMapper за директно мапиране към DTO
                .AsSplitQuery() // Използване на разделени заявки
                .ToListAsync();

            return result;
        }





    }
}

