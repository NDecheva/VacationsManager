using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Security;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]

    public class UserRepository : BaseRepository<User, UserDto>, IUserRepository
    {
        public UserRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> CanUserLoginAsync(string username, string password)
        {
            var userEntity = await _dbSet.FirstOrDefaultAsync(u => u.Username == username);

            if (userEntity == null)
            {
                return false;
            }

            return PasswordHasher.VerifyPassword(password, userEntity.Password);
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            return MapToModel(await _dbSet.FirstOrDefaultAsync(u => u.Username == username));
        }

        public async Task<IEnumerable<UserDto>> GetFreeTeamLeadersAsync()
        {
            var teamLeaderRoleId = await _context.Set<Role>()
                .Where(r => r.Name == "TeamLeader")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var assignedTeamLeaderIds = await _context.Set<Team>()
                .Select(t => t.TeamLeaderId)
                .ToListAsync();

            var teamLeaders = await _dbSet
                .Where(u => u.RoleId == teamLeaderRoleId &&
                            !assignedTeamLeaderIds.Contains(u.Id))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            return teamLeaders;
        }
    }
}