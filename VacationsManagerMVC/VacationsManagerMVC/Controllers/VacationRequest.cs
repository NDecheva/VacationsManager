using AutoMapper;
using Azure.Core;
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
                    Value = ((int)vacationType).ToString(), 
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
        public override async Task<IActionResult> List(int pageSize = DefaultPageSize, int pageNumber = DefaultPageNumber)
        {
            // Извикване на базовия метод
            var baseResult = await base.List(pageSize, pageNumber) as ViewResult;

            if (baseResult == null || !(baseResult.Model is IEnumerable<VacationRequestDetailsVM> paginatedRequests))
            {
                return BadRequest("Error loading paginated data.");
            }

            // Вземане на текущия потребител
            var currentUserId = User.Identity.Name;
            var currentUser = await _userService.GetByUsernameAsync(currentUserId);

            if (currentUser == null)
            {
                return Unauthorized("User not found.");
            }

            // Определяне на ролята на потребителя
            RoleType role = RoleType.Unassigned;
            if (User.IsInRole("Developer"))
            {
                role = RoleType.Developer;
            }
            else if (User.IsInRole("TeamLead"))
            {
                role = RoleType.TeamLead;
            }
            else if (User.IsInRole("CEO"))
            {
                role = RoleType.CEO;
            }

            // Получаване на филтрираните заявки от базата данни
            var query = await _vacationRequestService.GetRequestsByUserRoleAsync(currentUser, role);

            // Изчисляване на общия брой записи за избраната роля
            var totalRecords = query.Count();

            // Пагиниране на заявките
            var paginatedRequestsResult = query
                .OrderBy(r => r.Id) // Сортиране на заявките по ID за правилната пагинация
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // Извършваме пагинацията тук

            // Преобразуване в ViewModel
            var mappedModels = _mapper.Map<IEnumerable<VacationRequestDetailsVM>>(paginatedRequestsResult);

            // Изчисляване на общия брой страници
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Предаване на информация за пагинацията към View
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            return View(nameof(List), mappedModels);
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
                editVM = await PrePopulateVMAsync(editVM);
                return View("Create", editVM);
            }

            if (editVM.VacationType == VacationType.SickLeave && attachmentFile == null)
            {
                Console.WriteLine("Attachment is required for SickLeave.");
                ModelState.AddModelError("Attachment", "Sick leave requires an attachment.");
                editVM = await PrePopulateVMAsync(editVM);
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


        [HttpPost("VacationRequest/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, VacationRequestEditVM editVM, IFormFile AttachmentFile)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var existingRequest = await _service.GetByIdIfExistsAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Vacation request not found.");
            }

            try
            {
                editVM.RequesterId = existingRequest.RequesterId;

                if (AttachmentFile != null && AttachmentFile.Length > 0)
                {
                    var uploadsPath = Path.Combine("wwwroot/uploads");
                    Directory.CreateDirectory(uploadsPath);

                    var filePath = Path.Combine(uploadsPath, AttachmentFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AttachmentFile.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(existingRequest.Attachment))
                    {
                        var oldFilePath = Path.Combine(uploadsPath, existingRequest.Attachment);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    editVM.Attachment = AttachmentFile.FileName;
                }
                else
                {
                    editVM.Attachment = existingRequest.Attachment;
                }

                var vacationRequestDto = _mapper.Map<VacationRequestDto>(editVM);
                await _service.SaveAsync(vacationRequestDto);

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the vacation request.");
                editVM = await PrePopulateVMAsync(editVM);
                return View(editVM);
            }
        }




        [HttpGet]
        public IActionResult DownloadAttachment(string fileName)
        {
            try
            {
                var fileBytes = _vacationRequestService.DownloadAttachment(fileName);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Roles = "TeamLead,CEO")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                // Вземете заявката по ID
                var request = await _vacationRequestService.GetByIdIfExistsAsync(id);
                if (request == null)
                {
                    return NotFound("Vacation request not found.");
                }

                // Проверете дали потребителят е TeamLead и заявката е подадена от TeamLead
                var requester = await _userService.GetByIdIfExistsAsync(request.RequesterId);
                if (requester == null)
                {
                    return NotFound("Requester not found.");
                }

                // Сравнете свойството Name или друго поле от RoleDto със стринг
                if (requester.Role?.Name == "TeamLead" && !User.IsInRole("CEO"))
                {
                    return Forbid("Only a CEO can approve requests submitted by a TeamLead.");
                }

                // Одобряване на заявката
                await _vacationRequestService.ApproveRequestAsync(id);

                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


    }
}
