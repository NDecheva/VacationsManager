using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, Developer, TeamLead")]
    public class VacationRequestController : BaseCrudController<VacationRequestDto, IVacationRequestRepository, IVacationRequestService, VacationRequestEditVM, VacationRequestDetailsVM>
    {
        protected readonly IVacationRequestService _vacationRequestService;
        protected readonly IUserService _userService;


        public VacationRequestController(IVacationRequestService vacationRequestService, IMapper mapper, IUserService userService)
            : base(vacationRequestService, mapper)
        {
            this._userService = userService;
            this._vacationRequestService = vacationRequestService;
        }

        protected override async Task<VacationRequestEditVM> PrePopulateVMAsync(VacationRequestEditVM editVM)
        {
            editVM.Requesters = (await _userService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()));

            editVM.VacationTypes = (await _vacationRequestService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.VacationType}", x.Id.ToString()));

            return editVM;
        }
    }
}
