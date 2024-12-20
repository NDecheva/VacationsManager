using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface IUserService : IBaseCrudService<UserDto, IUserRepository>
    {
        Task<bool> CanUserLoginAsync(string username, string password);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllActiveAsync();
    }
}
