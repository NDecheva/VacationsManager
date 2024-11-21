using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using YourNamespace.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Repos.Contracts
{
    public interface IRoleRepository : IBaseRepository<RoleDto>
    {
        Task<RoleDto> GetByNameIfExistsAsync(string name);
    }
}
