using AutoMapper;
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
    }
}