using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, Developer, TeamLead")]
    public class NotificationController : BaseCrudController<NotificationDto, INotificationRepository, INotificationService, NotificationEditVM, NotificationDetailsVM>
    {
        protected readonly IUserService _userService;
        protected readonly INotificationService _notificationService; 


        public NotificationController(IMapper mapper, INotificationService notificationService, IUserService userService)
            : base(notificationService, mapper)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        protected override async Task<NotificationEditVM> PrePopulateVMAsync(NotificationEditVM editVM)
        {
            editVM.UserList = (await _userService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()));
           

            return editVM;
        }

        [HttpPost]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(notificationId);
                return Ok("Notification marked as read.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var unreadCount = (await _notificationService.GetUnreadNotificationsAsync(userId)).Count();
                return Json(unreadCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching unread notifications count: {ex.Message}");
                return StatusCode(500, "An error occurred.");
            }
        }


    }
}
