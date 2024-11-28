using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Data.Repos;
using VacationsManager.Shared.Dtos;

namespace VacationManager.Tests.Repos
{
    public class UserRepositoryTests : BaseRepositoryTests<UserRepository, User, UserDto>
    {
    }
}
