using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO, TeamLead, Developer")]
    public class VacationRequestController : BaseCrudController<VacationRequestDto, IVacationRequestRepository, IVacationRequestService, VacationRequestEditVM, VacationRequestDetailsVM>
    {
        private readonly IVacationRequestService _vacationRequestService;
        private readonly IUserService _userService;

        public VacationRequestController(IVacationRequestService vacationRequestService, IMapper mapper, IUserService userService)
            : base(vacationRequestService, mapper)
        {
            _vacationRequestService = vacationRequestService;
            _userService = userService;
        }

        protected override async Task<VacationRequestEditVM> PrePopulateVMAsync(VacationRequestEditVM editVM)
        {
            editVM.Requesters = (await _userService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Username}", x.Id.ToString()))
                .ToList();

            editVM.VacationTypes = Enum.GetValues(typeof(VacationType))
                .Cast<VacationType>()
                .Select(vacationType => new SelectListItem
                {
                    Value = vacationType.ToString(),
                    Text = vacationType.ToString()
                })
                .ToList();

            var currentUserId = User.Identity.Name; 
            var currentUser = await _userService.GetByUsernameAsync(currentUserId); 

            if (currentUser != null)
            {
                editVM.RequesterId = currentUser.Id; 
            }

            return editVM;
        }

        [HttpGet]
        public override async Task<IActionResult> List(
            int pageSize = DefaultPageSize,
            int pageNumber = DefaultPageNumber)
        {
            var currentUserId = User.Identity.Name;
            var currentUser = await _userService.GetByUsernameAsync(currentUserId);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var requests = await _service.GetAllAsync();

            if (User.IsInRole("Developer"))
            {
                requests = requests.Where(r => r.RequesterId == currentUser.Id).ToList();
            }
            //else if (User.IsInRole("TeamLead"))
            //{
            //    var teamMembers = await _userService.GetTeamMembersAsync(currentUser.TeamId);
            //    requests = requests.Where(r => teamMembers.Any(tm => tm.Id == r.RequesterId)).ToList();
            //}

            var mappedRequests = _mapper.Map<IEnumerable<VacationRequestDetailsVM>>(requests);
            return View("List", mappedRequests);
        }


        

        [HttpGet]
        [Route("VacationRequest/FilterByDate")]
        public async Task<IActionResult> FilterByDate(DateTime? startDate)
        {
            ViewBag.FilterDate = startDate?.ToString("yyyy-MM-dd");

            if (startDate == null)
            {
                return RedirectToAction(nameof(List));
            }

            var vacationRequests = await _vacationRequestService.GetAllAsync();
            var filteredRequests = vacationRequests
                .Where(v => v.StartDate >= startDate.Value)
                .ToList();

            var vacationRequestVMs = _mapper.Map<IEnumerable<VacationRequestDetailsVM>>(filteredRequests);

            return View("List", vacationRequestVMs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWithAttachment(VacationRequestEditVM editVM, IFormFile attachmentFile)
        {
            var errors = await Validate(editVM);
            if (errors != null)
            {
                return View("Create", editVM);
            }

            if (editVM.SelectedVacationType == VacationType.SickLeave && attachmentFile == null)
            {
                ModelState.AddModelError("Attachment", "Sick leave requires an attachment.");
                return View("Create", editVM);
            }

            if (attachmentFile != null && attachmentFile.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/uploads", attachmentFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachmentFile.CopyToAsync(stream);
                }
                editVM.Attachment = attachmentFile.FileName; 
            }

            var vacationRequestDto = _mapper.Map<VacationRequestDto>(editVM);
            await _service.SaveAsync(vacationRequestDto);

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult DownloadAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required.");
            }

            var filePath = Path.Combine("wwwroot/uploads", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream";
            return File(fileBytes, contentType, fileName);
        }
    }
}
