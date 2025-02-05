using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class NotificationService : BaseCrudService<NotificationDto, INotificationRepository>, INotificationService
    {
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task SendNotificationAsync(int recipientId, string message)
        {
            // Създайте обект Notification и го мапнете към NotificationDto
            var notification = new NotificationDto
            {
                RecipientId = recipientId,
                Message = message,
                DateSent = DateTime.UtcNow,
                IsRead = false
            };

            var notificationDto = _mapper.Map<NotificationDto>(notification);

            // Запишете мапнатия DTO в базата данни
            await _repository.SaveAsync(notificationDto);
        }


        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _repository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new Exception("Notification not found.");
            }

            notification.IsRead = true;
            await _repository.SaveAsync(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync(int userId)
        {
            return await _repository.GetUnreadNotificationsAsync(userId);
        }
    }
}