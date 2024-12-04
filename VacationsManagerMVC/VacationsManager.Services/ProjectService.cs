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
    public class ProjectService : BaseCrudService<ProjectDto,  IProjectRepository>, IProjectService
    {
        public ProjectService(IProjectRepository repository) : base(repository) { }
    }
}