using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class RoleService : BaseCrudService<RoleDto, IRoleRepository>, IRoleService
    {
        public RoleService(IRoleRepository repository) : base(repository)
        {

        }
<<<<<<< HEAD

        public Task<RoleDto> GetByNameIfExistsAsync(string name)
        {
            return _repository.GetByNameIfExistsAsync(name);
        }

=======
        public Task<RoleDto?> GetByNameIfExistsAsync(string name)
        {
            return _repository.GetByNameIfExistsAsync(name);

        }
>>>>>>> 2e0bf2d (VM-33 Created services)

    }
}