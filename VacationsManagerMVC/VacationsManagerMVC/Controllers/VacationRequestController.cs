using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
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
        private readonly ILogger<VacationRequestController> _logger;

        public VacationRequestController(
            IVacationRequestService vacationRequestService, 
            IMapper mapper, 
            IUserService userService,
            ILogger<VacationRequestController> logger)
            : base(vacationRequestService, mapper)
        {
            _vacationRequestService = vacationRequestService;
            _userService = userService;
            _logger = logger;
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
            try
            {
                var currentUserId = User.Identity.Name;
                var currentUser = await _userService.GetByUsernameAsync(currentUserId);
                if (currentUser == null) return Unauthorized("User not found.");

                RoleType role = (RoleType)currentUser.Role.Id;

                var result = await _vacationRequestService.GetPaginatedRequestsAsync(currentUser, role, pageSize, pageNumber);
                var mappedModels = _mapper.Map<IEnumerable<VacationRequestDetailsVM>>(result.PaginatedRequests);

                ViewBag.TotalPages = result.TotalPages;
                ViewBag.CurrentPage = pageNumber;

                return View(nameof(List), mappedModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vacation request list");
                return BadRequest("Error loading data.");
            }
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

            try
            {
                var currentUserId = User.Identity.Name;
                var currentUser = await _userService.GetByUsernameAsync(currentUserId);
                if (currentUser == null) return Unauthorized("User not found.");

                RoleType role = (RoleType)currentUser.Role.Id;

                var vacationRequests = await _vacationRequestService.GetRequestsByDateAsync(currentUser, role, startDate.Value);
                var vacationRequestVMs = _mapper.Map<IEnumerable<VacationRequestDetailsVM>>(vacationRequests);

                return View("List", vacationRequestVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering vacation requests by date");
                return BadRequest("Error filtering data.");
            }
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

            var isValid = await _vacationRequestService.ValidateVacationTypeRequiresAttachmentAsync(editVM.VacationType, attachmentFile);
            if (!isValid)
            {
                ModelState.AddModelError("Attachment", "Sick leave requires an attachment.");
                editVM = await PrePopulateVMAsync(editVM);
                return View("Create", editVM);
            }

            if (attachmentFile != null)
            {
                editVM.Attachment = await _vacationRequestService.SaveAttachmentAsync(attachmentFile);
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

                if (AttachmentFile != null)
                {
                    await _vacationRequestService.UpdateAttachmentAsync(id, AttachmentFile, existingRequest.Attachment);
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
                _logger.LogError(ex, "Error updating vacation request");
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
                _logger.LogError(ex, "Error downloading attachment");
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "TeamLead,CEO")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var currentUserId = User.Identity.Name;
                var currentUser = await _userService.GetByUsernameAsync(currentUserId);
                if (currentUser == null) return Unauthorized("User not found.");

                var canApprove = await _vacationRequestService.CanUserApproveRequestAsync(currentUser, id);
                if (!canApprove)
                {
                    return Forbid("Only a CEO can approve requests submitted by a TeamLead.");
                }

                await _vacationRequestService.ApproveRequestAsync(id);

                return Json(new { success = true, message = "Vacation request approved successfully.", requestId = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving vacation request");
                return BadRequest(ex.Message);
            }
        }
    }
}
