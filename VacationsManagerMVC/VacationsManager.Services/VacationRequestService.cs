using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class VacationRequestService : BaseCrudService<VacationRequestDto, IVacationRequestRepository>, IVacationRequestService
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly string _uploadsPath = "wwwroot/uploads";

        public VacationRequestService(
            IVacationRequestRepository repository,
            IUserService userService,
            INotificationService notificationService)
            : base(repository)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task ApproveRequestAsync(int id)
        {
            var request = await _repository.GetByIdAsync(id);
            if (request == null)
            {
                throw new Exception("Vacation request not found.");
            }

            if (request.IsApproved)
            {
                throw new Exception("Vacation request is already approved.");
            }

            request.IsApproved = true;
            await _repository.SaveAsync(request);

            // Изпрати нотификация
            var message = $"Your vacation request from {request.StartDate:yyyy-MM-dd} to {request.EndDate:yyyy-MM-dd} has been approved.";
            await _notificationService.SendNotificationAsync(request.RequesterId, message);
        }

        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByUserRoleAsync(UserDto currentUser, RoleType role)
        {
            return await _repository.GetRequestsByUserRoleAsync(currentUser, role);
        }

        public async Task<IEnumerable<VacationRequestDto>> GetRequestsByDateAsync(UserDto currentUser, RoleType role, DateTime startDate)
        {
            return await _repository.GetRequestsByDateAsync(currentUser, role, startDate);
        }

        public byte[] DownloadAttachment(string fileName)
        {
            var filePath = Path.Combine(_uploadsPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new Exception("File not found.");
            }

            return File.ReadAllBytes(filePath);
        }
        
        public async Task<string> SaveAttachmentAsync(IFormFile attachmentFile)
        {
            if (attachmentFile == null || attachmentFile.Length == 0)
            {
                return null;
            }

            Directory.CreateDirectory(_uploadsPath);
            var filePath = Path.Combine(_uploadsPath, attachmentFile.FileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachmentFile.CopyToAsync(stream);
            }
            
            return attachmentFile.FileName;
        }
        
        public async Task UpdateAttachmentAsync(int requestId, IFormFile attachmentFile, string existingAttachmentName)
        {
            if (attachmentFile == null || attachmentFile.Length == 0)
            {
                return;
            }
            
            Directory.CreateDirectory(_uploadsPath);
            
            // Save new file
            var filePath = Path.Combine(_uploadsPath, attachmentFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachmentFile.CopyToAsync(stream);
            }
            
            // Delete old file if it exists
            if (!string.IsNullOrEmpty(existingAttachmentName))
            {
                var oldFilePath = Path.Combine(_uploadsPath, existingAttachmentName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
        }
        
        public Task<bool> ValidateVacationTypeRequiresAttachmentAsync(VacationType vacationType, IFormFile attachmentFile)
        {
            // Check if attachment is required for this vacation type
            if (vacationType == VacationType.SickLeave && (attachmentFile == null || attachmentFile.Length == 0))
            {
                return Task.FromResult(false);
            }
            
            return Task.FromResult(true);
        }
        
        public async Task<(IEnumerable<VacationRequestDto> PaginatedRequests, int TotalPages)> GetPaginatedRequestsAsync(
            UserDto currentUser, RoleType role, int pageSize, int pageNumber)
        {
            var vacationRequests = await GetRequestsByUserRoleAsync(currentUser, role);
            var totalRecords = vacationRequests.Count();
            
            var paginatedRequests = vacationRequests
                .OrderBy(r => r.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            
            return (paginatedRequests, totalPages);
        }
        
        public async Task<bool> CanUserApproveRequestAsync(UserDto currentUser, int requestId)
        {
            var request = await GetByIdIfExistsAsync(requestId);
            if (request == null)
            {
                throw new Exception("Vacation request not found.");
            }
            
            var requester = await _userService.GetByIdIfExistsAsync(request.RequesterId);
            if (requester == null)
            {
                throw new Exception("Requester not found.");
            }
            
            // Only CEO can approve TeamLead requests
            if (requester.Role?.Name == "TeamLead" && currentUser.Role?.Name != "CEO")
            {
                return false;
            }
            
            return true;
        }
    }
}
