using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using YourNamespace.Shared.Repos.Contracts;

namespace VacationManager.Tests.Services
{
    public abstract class BaseServiceTests<TModel, TRepository, TService>
where TModel : BaseModel
where TRepository : IBaseRepository<TModel>
        where TService : BaseCrudService<TModel, TRepository>
    {
        protected readonly TRepository _repository;
        protected BaseServiceTests(TRepository repository)
        {
            this._repository = repository;
        }
    }
}