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
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class RoleRepository : BaseRepository<Role, RoleDto>, IRoleRepository
    {
        public RoleRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<RoleDto?> GetByNameIfExistsAsync(string name)
        {
            return MapToModel(await _dbSet.FirstOrDefaultAsync(u => u.Name == name));

        }
    }
}