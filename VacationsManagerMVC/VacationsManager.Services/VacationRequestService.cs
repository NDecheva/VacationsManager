using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class VacationRequestService : BaseCrudService<VacationRequestDto, IVacationRequestRepository>, IVacationRequestService
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

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

        public byte[] DownloadAttachment(string fileName)
        {
            var filePath = Path.Combine("wwwroot/uploads", fileName);
            if (!File.Exists(filePath))
            {
                throw new Exception("File not found.");
            }

            return File.ReadAllBytes(filePath);
        }
    }
}
