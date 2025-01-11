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

        public VacationRequestService(IVacationRequestRepository repository, IUserService userService) : base(repository)
        {
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
