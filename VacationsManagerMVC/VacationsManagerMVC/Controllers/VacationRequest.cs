using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, Developer, TeamLead")]
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
            // Вземаме всички потребители
            editVM.Requesters = (await _userService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()))
                .ToList();

            // Попълваме VacationTypes
            editVM.VacationTypes = Enum.GetValues(typeof(VacationType))
                .Cast<VacationType>()
                .Select(vacationType => new SelectListItem
                {
                    Value = vacationType.ToString(),
                    Text = vacationType.ToString()
                })
                .ToList();

            // Задаваме RequesterId на текущия логнат потребител
            var currentUserId = User.Identity.Name; // или User.Identity.GetUserId(), ако използвате ASP.NET Identity
            var currentUser = await _userService.GetByUsernameAsync(currentUserId); // Ако имате метод за вземане на потребител по username

            // Уверете се, че currentUser != null и че използвате правилното свойство (например Id)
            if (currentUser != null)
            {
                editVM.RequesterId = currentUser.Id; // Присвояваме Id на RequesterId
            }

            return editVM;
        }


    }
}
