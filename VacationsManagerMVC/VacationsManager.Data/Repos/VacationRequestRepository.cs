using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VacationsManager.Data.Repos.VacationRequestRepository;
using VacationsManager.Shared.Attributes;
using YourNamespace.Data.Repos;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class VacationRequestRepository : BaseRepository<VacationRequest, VacationRequestDto>, IVacationRequestRepository
    {
        public VacationRequestRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}

