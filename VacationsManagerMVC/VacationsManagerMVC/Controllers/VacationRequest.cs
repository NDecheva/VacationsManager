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


            Console.WriteLine("VacationTypes loaded:");
            foreach (var type in editVM.VacationTypes)
            {
                Console.WriteLine($"Value: {type.Value}, Text: {type.Text}");
            }

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
            else if (User.IsInRole("TeamLead"))
            {
                if (currentUser.TeamId.HasValue)
                {
                    var teamMembers = await _userService.GetTeamMembersAsync(currentUser.TeamId.Value);

                    requests = requests.Where(r =>
                            r.RequesterId == currentUser.Id ||
                            teamMembers.Any(tm => tm.Id == r.RequesterId))
                        .ToList();
                }
                else
                {
                    requests = requests.Where(r => r.RequesterId == currentUser.Id).ToList();
                }
            }

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
            Console.WriteLine($"VacationType in editVM (SelectedVacationType): {editVM.VacationType}");

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
            Console.WriteLine($"VacationType in DTO: {vacationRequestDto.VacationType}");

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
