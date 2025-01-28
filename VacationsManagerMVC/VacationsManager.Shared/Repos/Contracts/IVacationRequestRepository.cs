using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using YourNamespace.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Repos.Contracts
{
    public interface IVacationRequestRepository:IBaseRepository<VacationRequestDto>
    {
        Task<IEnumerable<VacationRequestDto>> GetRequestsByUserRoleAsync(UserDto currentUser, RoleType role);
    }
}
