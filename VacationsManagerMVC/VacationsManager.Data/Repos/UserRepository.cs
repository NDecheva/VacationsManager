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
            var query = @"
        SELECT u.Id, u.FirstName, u.LastName
        FROM dbo.Users u
        WHERE u.Id NOT IN (
            SELECT DISTINCT t.TeamLeaderId 
            FROM dbo.Teams t
        )
        AND u.RoleId = @TeamLeaderRoleId
    ";

            // Execute the query using FromSqlRaw, ensuring it's recognized as part of the Users DbSet.
            var teamLeaders = await _context.Set<User>()
                .FromSqlRaw(query, new SqlParameter("@TeamLeaderRoleId", 3)) // Assuming RoleId = 3 is for Team Leaders
                .ToListAsync();

            // Map the result to DTO
            var teamLeaderDtos = teamLeaders.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();

            return teamLeaderDtos;
        }
    }
}