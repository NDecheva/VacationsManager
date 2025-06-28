using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface IVacationRequestService : IBaseCrudService<VacationRequestDto, IVacationRequestRepository>
    {
        Task ApproveRequestAsync(int id);

        byte[] DownloadAttachment(string fileName);

        Task<IEnumerable<VacationRequestDto>> GetRequestsByUserRoleAsync(UserDto currentUser, RoleType role);

        Task<IEnumerable<VacationRequestDto>> GetRequestsByDateAsync(UserDto currentUser, RoleType role, DateTime startDate);
        
        Task<string> SaveAttachmentAsync(IFormFile attachmentFile);
        
        Task UpdateAttachmentAsync(int requestId, IFormFile attachmentFile, string existingAttachmentName);
        
        Task<bool> ValidateVacationTypeRequiresAttachmentAsync(VacationType vacationType, IFormFile attachmentFile);
        
        Task<(IEnumerable<VacationRequestDto> PaginatedRequests, int TotalPages)> GetPaginatedRequestsAsync(
            UserDto currentUser, RoleType role, int pageSize, int pageNumber);
            
        Task<bool> CanUserApproveRequestAsync(UserDto currentUser, int requestId);
    }
}
