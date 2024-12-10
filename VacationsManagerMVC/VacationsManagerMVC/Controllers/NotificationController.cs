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
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin, Employee, User")]
    public class NotificationController : BaseCrudController<NotificationDto, INotificationRepository, INotificationService, NotificationEditVM, NotificationDetailsVM>
    {
        protected readonly IUserService _userService;


        public NotificationController(IMapper mapper, INotificationService notificationService, IUserService userService)
            : base(notificationService, mapper)
        {
            _userService = userService;
        }

        protected override async Task<NotificationEditVM> PrePopulateVMAsync(NotificationEditVM editVM)
        {
            editVM.UserList = (await _userService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()));
           

            return editVM;
        }
    }
}
