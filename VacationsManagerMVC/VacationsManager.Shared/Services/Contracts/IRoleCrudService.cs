using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface IRoleCrudService : IBaseCrudService<RoleDto, IRoleRepository>

    {
        Task<RoleDto> GetByNameIfExistsAsync(string name);
    }
}
